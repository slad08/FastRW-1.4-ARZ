using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FastRW
{
    public partial class Form1 : Form
    {
        #region Vars

        string connection_String = ConfigurationManager.ConnectionStrings["AdoConStr"].ConnectionString;

        int varOnOff;
        int varWrite;
        int varOnReadMan;

        short varHide;

        int ConnectRevise;

        short id_obj;
        short id_steps;
        short id_isp;
       

        //short sizeBlock = 22;
        //short CountBlock = 2;

        // string regMass = "D5095"; //D5095 id_isp D5097 массив значений флагов 
        string regMass = "D2980"; //TEST_SEL id_isp в SCADA адрес в PLC D2187
        short[] mass = new short[20];

        string regMassSteps = "D2107"; //TEST_SEL id_isp в SCADA адрес в PLC D2187
        short[] massSteps = new short[20];
      
        //Запись в базу из PLC в ручном режиме

        //флаг на считывания данных с плк в ручном режиме левая сторорна в SCADA MAN_START_LEFT
        string ManStartLeft = "D2191";
        short ManStartLeftVal = 0;

        ////флаг на считывания данных с плк в ручном режиме правая сторорна в SCADA MAN_START_RIGHT
        //string ManStartRight = "D2192";
        //short ManStartRightVal = 0;

        //блок параметров из плк в ручном режиме для левой стороны
        string regManParam1 = "D2248";
        short[] regManParam1Val = new short[30];

        ////блок общих параметров 
        //string regManParamGener = "D2286";
        //short[] regManParamGenerVal = new short[20];

        ////блок параметров из плк в ручном режиме для правой стороны
        //string regManParamRight = "D2266";
        //short[] regManParamRightVal = new short[30];

        //Старый вариант
        //string regManParam2 = "D402";
        //short[] regManParam2Val = new short[50];

        //Запись в базу из PLC в автоматическом режиме READ AUTO

        string regAutoRead = "M2128";//флаг на считывания данных с плк в автомате ВЗЯТЬ У ЮРЫ адрес в PLC М2128
        short regAutoReadVal = 0;

        string regAutoReadEnd = "D2162";//флаг конца считывания блока в автомате ВЗЯТЬ У ЮРЫ адрес в PLC D2129
        short regAutoReadEndVal = 0;

        ////Запись в базу из PLC в автоматическом режиме READ AUTO
        //string regAutoRead2 = "M2132";// ЗАМЕНИТЬ флаг на считывания данных с плк в автомате ПРАВАЯ СТОРОНА
        //short regAutoRead2Val = 0;

        string regAutoReadEnd2 = "D2164";//ЗАМЕНИТЬ флаг конца считывания блока в автомате ПРАВАЯ СТОРОНА
        short regAutoReadEnd2Val = 0;

        //блок параметров из плк в Автоматическом режиме
        // string regParams1 = "D2200";//считывание параметров из плк
        string regParams1 = "D5000";//считывание параметров из плк
        short[] regParams1Val = new short[100];

        ////блок параметров из плк в Автоматическом режиме для правой стороны
        ////  string regParams2 = "D2218";//считывание параметров из плк
        //string regParams2 = "D2216";//считывание параметров из плк
        //short[] regParams2Val = new short[30];

        ////блок параметров из плк в Автоматическом режиме для обеих сторон
        //string regParams3 = "D2236";//считывание параметров из плк
        //short[] regParams3Val = new short[20];


        //string regCalibrStart = "D5100";
        //short regCalibrStartVal = 0;

        //string regCalibr = "D6650";
        //short[] regCalibrVal = new short[111];

        ////Флаг на запись Технологических Аварий в PLC
        //string regSensWriteStart = "D2189";
        //short regSensWriteStartVal = 0;

        ////Флаг на запись датчиков TPMS в PLC
        //string regSensWriteTPMS = "D2190";
        //short regSensWriteTpmsVal = 0;

        //Регистр для записи датчиков TPMS в PLC
        string regSensTPMSNum = "D1355";
        // int[] regSensTPMSNumVal = new int[100];
        short[] regSensTPMSNumVal = new short[100];

        //string regSensorsMin = "D4600";//датчики
        //short[] regSensorsMinVal = new short[50];
        //string regSensorsMax = "D4700";//датчики
        //short[] regSensorsMaxVal = new short[50];
        //string regSensorsDeistv = "D4800";//датчики
        //short[] regSensorsDeistvVal = new short[50];


        //string regStartWrite = "D2180"; //D2180 флаг начала записи параметров в плк LOAD_NEED в SCADA
        //short regStartWriteVal = 0;

        ////Запись  шагов в PLC в АВТОМАТЕ Левая сторона
        //string regStartWrite = "D2193"; //флаг начала записи параметров в плк LOAD_NEED_1 в SCADA
        //short regStartWriteVal = 0;

        string regStopWrite = "D2163";//флаг на завершение записи параметров в плк
        short regStopWriteVal = 0;

        //Запись  шагов в PLC в АВТОМАТЕ Правая сторона
        string regStartWrite2 = "D2194"; //флаг начала записи параметров в плк LOAD_NEED_1 в SCADA
        short regStartWrite2Val = 0;

        string regStopWrite2 = "D2165";//флаг на завершение записи параметров в плк
        short regStopWrite2Val = 0;

        string regConfig = "D6005";//конфиг стенда
        short[] regConfigVal = new short[10];

        //string regSteps = "D4001";//шаги испытания
        //short[] regStepsVal = new short[1500];

        string regSteps = "R0";//шаги испытания шина слева 1 с D0-D15
        short[] regStepsVal = new short[1500];

        //string regSteps2 = "R2000";//шаги испытания шина слева 1 с D2000-D2019
        //short[] regStepsVal2 = new short[1500];

        //  int[] regStepsVal = new int[1500];

        //   string regSteps1 = "R8";//шаги испытания испытания шина слева 1 с D8-D15
        //   short[] regSteps1Val = new short[1500];


        int messInitCalibr = 0;
        #endregion

        public Form1()
        {
            InitializeComponent();

            #region MyRegion
            /*
            int intAgain1 = int.Parse("a2f", System.Globalization.NumberStyles.HexNumber);
            Single intAgain1_1 = Single.Parse(intAgain1.ToString());
            int intAgain2 = int.Parse("a3f", System.Globalization.NumberStyles.HexNumber);
            Single intAgain2_1 = Single.Parse(intAgain2.ToString());
            */
            #endregion

            timerTime.Start();

            try
            {
                ConnectRevise = MxComp.Open();
                if (ConnectRevise == 0)
                {
                    label1.Text = "Соединение успешно";
                    timerStart.Start();
                    Main_timer.Start();
                }
                else
                {
                    label1.Text = "Нет связи";
                    MessageBox.Show("FastRW не смог установить свять с контроллером!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " open");
            }
        }

        #region Int to Float
        private Single TwoIntToFloat(short a, short b)
        {
            int temp;

            temp = ((int)a) & 0x0000ffff;
            temp = temp | (((int)b) << 16);

            float f = BitConverter.ToSingle(BitConverter.GetBytes((temp)), 0);
            return f;
        }
        #endregion

        #region Float to INT
        private void FloatToTwoInt(Single c, ref short a, ref short b)
        {
            int temp;

            temp = BitConverter.ToInt32(BitConverter.GetBytes(c), 0);

            a = (short)temp;
            b = (short)(temp >> 16);
        }
        #endregion

        #region Zaderghka na minimize
        private void timerStart_Tick(object sender, EventArgs e)
        {
            if (varHide == 10)
            {
                varHide = 0;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                timerStart.Stop();
            }

            varHide++;
        }
        #endregion

        #region Hide or activate form
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
        }
        #endregion

        private void Main_timer_Tick(object sender, EventArgs e)
        {

            #region Register On read in auto
            try
            {
                MxComp.GetDevice2(regAutoRead, out regAutoReadVal);//проверка идентификатора на начало чтения в автомате левая сторона
                MxComp.GetDevice2(regAutoReadEnd, out regAutoReadEndVal);//проверка идентификатора на начало чтения в автомате левая сторона

                //    MxComp.GetDevice2(regAutoRead2, out regAutoRead2Val);//проверка идентификатора на начало чтения в автомате левая сторона
                MxComp.GetDevice2(regAutoReadEnd2, out regAutoReadEnd2Val);//проверка идентификатора на начало чтения в автомате левая сторона


                MxComp.GetDevice2(ManStartLeft, out ManStartLeftVal);//проверка идентификатора на начало чтения в ручном для левой стороны
                                                                     //  MxComp.GetDevice2(ManStartRight, out ManStartRightVal);//проверка идентификатора на начало чтения в ручном для правой стороны

                // MxComp.GetDevice2(regSensWriteStart, out regSensWriteStartVal);
                // MxComp.GetDevice2(regStartWrite, out regStartWriteVal);//проверка идентификатора на начало записи ЛЕВО
                MxComp.GetDevice2(regStartWrite2, out regStartWrite2Val);//проверка идентификатора на начало записи ПРАВО


           //     MxComp.GetDevice2(regCalibrStart, out regCalibrStartVal);

                //  MxComp.GetDevice2(regSensWriteTPMS, out regSensWriteTpmsVal);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " main_timer Register On read in auto");
            }
            #endregion

            #region id_tire_1; id_isp; id_steps; id_steps_2; 
            try
            {
                MxComp.ReadDeviceBlock2(regMass, 20, out mass[0]);

              //  MxComp.ReadDeviceBlock2(regMassSteps, 4, out massSteps[0]);

                //id_tire_1 = mass[0];// ID_PROD1 В SCADA
                //id_tire_2 = mass[1];// ID_PROD2 В SCADA
                //id_isp = mass[2]; // ID_ISP_1 в SCADA 
                //id_isp_2 = mass[3];// ID_ISP_2 в SCADA 

                //id_steps = massSteps[0];//TEK_SHAG_1 id шага D2107 в SCADA
                //id_steps_2 = massSteps[2];//TEK_SHAG_2 id шага D2109 в SCADA

                
                id_steps = mass[0];//ID_STEP id шага D2980 в SCADA
                id_obj = mass[3];// ID_PROD D2983 в SCADA
                id_isp = mass[4]; // ID_ISP D2984 в SCADA 

                
                //  id_type = mass[4];// в SCADA ID_TYPE id типа генератора в PLC D2188

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " main_timer");
            }
            //    MessageBox.Show("id_steps 1-й вызов " + id_steps.ToString());
            #endregion

            //Чтение в ручном режиме для левой стороны
            #region Read Params from PLC in man Left side
            /*
            if (ManStartLeftVal == 1)
            {
                  //чтение данных из контроллера  в ручном режиме 
                try
                {
                    MxComp.ReadDeviceBlock2(regManParam1, 18, out regManParam1Val[0]);
                    MxComp.ReadDeviceBlock2(regManParamGener, 6, out regManParamGenerVal[0]);
                    ManStartLeftVal = 0;
                    MxComp.SetDevice2(ManStartLeft, ManStartLeftVal);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " main_timer read params man");
                }

                //запис в БД данных из контроллера  в ручном режиме
                ReadManParamLeft(regManParam1Val, regManParamGenerVal, id_tire_1);
              
            }
            */
            #endregion

            //Чтение в ручном режиме для правой стороны
            #region Read Params from PLC in man Right side
            /*
            if (ManStartRightVal == 1)
            {
                 //чтение данных из контроллера  в ручном режиме 
                try
                {
                    MxComp.ReadDeviceBlock2(regManParamRight, 20, out regManParamRightVal[0]);
                    MxComp.ReadDeviceBlock2(regManParamGener, 6, out regManParamGenerVal[0]);
                    ManStartRightVal = 0;
                    MxComp.SetDevice2(ManStartRight, ManStartRightVal);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " main_timer read params man");
                }
                //запис в БД данных из контроллера  в ручном режиме
                ReadManParamRight(regManParamRightVal, regManParamGenerVal, id_tire_2);
               
            }
            */
            #endregion
            
            //Чтение   в автом-м режиме для левой стороны
            #region Read Params from PLC in auto Left Side
            if (regAutoReadVal == 1 && regAutoReadEndVal == 0)
            {
                try
                {
                    MxComp.ReadDeviceBlock2(regParams1, 80, out regParams1Val[0]);
                   // MxComp.ReadDeviceBlock2(regParams3, 6, out regParams3Val[0]);

                    regAutoReadEndVal = 1;
                    MxComp.SetDevice2(regAutoReadEnd, regAutoReadEndVal);
                    //     MxComp.ReadDeviceBlock2(regParams1, 40, out regParamsVal1[0]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " main_timer read params auto");
                }
                #region Insert data in db
                //запис в БД данных из контроллера  в автом-м режиме
              //  ReadParams(regParams1Val, regParams3Val, id_tire_1, id_steps, id_isp);
                ReadParams(regParams1Val, id_obj, id_steps, id_isp); 
                #endregion
            }
            #endregion

            //Чтение в автом-м режиме для правой стороны
            #region Read Params from PLC in auto Right Side
            /*
            if (regAutoRead2Val == 1 && regAutoReadEnd2Val == 0)
            {
                try
                {
                  //  MxComp.ReadDeviceBlock2(regParams2, 18, out regParams2Val[0]);
                    MxComp.ReadDeviceBlock2(regParams2, 20, out regParams2Val[0]);
                    MxComp.ReadDeviceBlock2(regParams3, 6, out regParams3Val[0]);

                    regAutoReadEnd2Val = 1;
                    MxComp.SetDevice2(regAutoReadEnd2, regAutoReadEnd2Val);
                    //     MxComp.ReadDeviceBlock2(regParams1, 40, out regParamsVal1[0]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " main_timer read params auto right side");
                }
             
                //запис в БД данных из контроллера  в автом-м режиме
              ReadParams2(regParams2Val, regParams3Val, id_tire_2, id_steps_2, id_isp_2);
            }
            */
            #endregion

            //Запись шагов в PLC левая сторона
            #region Write Steps
            /*
            if (regStartWriteVal == 1)
            {
                //запись данных в контроллер  в автом-м режиме ЛЕВАЯ СТОРОНА
                int count = 0;
                WriteSteps(id_isp, ref regStepsVal,ref count);

                try
                {
                  MxComp.WriteDeviceBlock2(regSteps, count, ref regStepsVal[0]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " main_timer");
                }
                regStartWriteVal = 0;
                regStopWriteVal = 1;

                MxComp.SetDevice2(regStartWrite, regStartWriteVal);
                MxComp.SetDevice2(regStopWrite, regStopWriteVal);
            }
            */
            #endregion

            //Правая сторона запись шагов в PLC
            #region Write Steps2
            /*
            if (regStartWrite2Val == 1)
            {
                //запись данных в контроллер  в автом-м режиме ПРАВАЯ СТОРОНА
                int count2 = 0;

                WriteSteps2(id_isp_2, ref regStepsVal2, ref count2);

                try
                {
                    //   MxComp.WriteDeviceBlock2(regSteps, 8, ref regStepsVal[0]);
                    //  MxComp.WriteDeviceBlock2(regSteps1, 8, ref regSteps1Val[0]);

                    MxComp.WriteDeviceBlock2(regSteps2, count2, ref regStepsVal2[0]);
                    // MxComp.WriteDeviceBlock2(regSteps1, count, ref regSteps1Val[0]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " main_timer");
                }
                regStartWrite2Val = 0;
                regStopWrite2Val = 1;

                MxComp.SetDevice2(regStartWrite2, regStartWrite2Val);
                MxComp.SetDevice2(regStopWrite2, regStopWrite2Val);
            }
            */
            #endregion

            #region Write sensors params
            /*
            if (regSensWriteStartVal == 1)
            {
                try
                {
                    WriteSensor(ref regSensorsMinVal, ref regSensorsMaxVal, ref regSensorsDeistvVal);

                    MxComp.WriteDeviceBlock2(regSensorsMin, 38, ref regSensorsMinVal[0]);
                    MxComp.WriteDeviceBlock2(regSensorsMax, 38, ref regSensorsMaxVal[0]);
                    MxComp.WriteDeviceBlock2(regSensorsDeistv, 19, ref regSensorsDeistvVal[0]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "sensor");
                }
                regSensWriteStartVal = 0;
             
                MxComp.SetDevice2(regSensWriteStart, regSensWriteStartVal);
            }
            */
            #endregion

            //Залить в PLC протестировать!
            #region Write sensors TPMS params 
            /*
            if (regSensWriteTpmsVal == 1)
            {
                try
                {
                    //      WriteSensorTPMS(ref regSensTPMSNumVal);
                    WriteSensorTPMS(ref regSensTPMSNumVal);

                    MxComp.WriteDeviceBlock2(regSensTPMSNum, 40, ref regSensTPMSNumVal[0]);
                    //  MxComp.WriteDeviceBlock(regSensTPMSNum, 40, ref regSensTPMSNumVal[0]);

                    //       MessageBox.Show("regSensWriteTpmsVal " + regSensTPMSNumVal);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "sensorTPMS");
                }
                regSensWriteTpmsVal = 0;
                MxComp.SetDevice2(regSensWriteTPMS, regSensWriteTpmsVal);

            }
            */
            #endregion

            #region Write calibr param
            /*
            if (regCalibrStartVal == 1)
            {

               
                try
                {
                    WriteCalibr(ref regCalibrVal);

                    MxComp.WriteDeviceBlock2(regCalibr, 111, ref regCalibrVal[0]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " calibr");
                }
               

                regCalibrStartVal = 0;
                MxComp.SetDevice2(regCalibrStart, regCalibrStartVal);

            }
            */
            #endregion
        }
        //Этот экран сейчас не используется, сделан на панеле оператора.
        #region Sensors TPMS
        /*
        private void WriteSensorTPMS(ref short[] SensNum)
        //private void WriteSensorTPMS(ref int[] SensNum)

        {

            DataSet dsSens = new DataSet();
            dsSens.Clear();

            string comm = " SELECT id, disk_num, ser_num FROM Sens_TPMS order by id";
            //  MessageBox.Show(comm);
            using (SqlConnection connection = new SqlConnection(connection_String))
            {
                connection.Open();
                FillSQLCommand(connection, comm, ref dsSens);
            }

            try
            {
                for (int i = 0; i < dsSens.Tables[0].Rows.Count; i++)
                {

                    #region MyRegion

                    // Convert the hex string back to the number
                    //int intAgain = int.Parse(dsSens.Tables[0].Rows[i]["ser_num"].ToString(), System.Globalization.NumberStyles.HexNumber);
                    //SensNum[2 * i] = intAgain;

                    //Рабочий кусок преобразование Hex во real РАСКОМЕНТИТЬ

                    int intAgain = int.Parse(dsSens.Tables[0].Rows[i]["ser_num"].ToString(), System.Globalization.NumberStyles.HexNumber);
                    FloatToTwoInt(Single.Parse(intAgain.ToString()), ref SensNum[2 * i], ref SensNum[2 * i + 1]);

                    #region test hex to int32 //ЗАКОМЕНТИТЬ
                    //Преобразование HEX to INT32 Тестирование функции преобразования HEX в Int32 (удалить после тестирования)
                    //   int test = int.Parse(dsSens.Tables[0].Rows[i]["ser_num"].ToString(), System.Globalization.NumberStyles.HexNumber);
                    //   SensNum[2*i] = (short)(Convert.ToInt32(test));

                    #endregion


                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        */
        #endregion

        #region Sensors Write
        /*
        private void WriteSensor(ref short[] SensMin, ref short[] SensMax, ref short[] SensDeistv)
        {
            DataSet dsSens = new DataSet();
            dsSens.Clear();
            //Аварии не привязаны к Типам шин, поэтому iDT = 1
            short idT = 1;

            string comm = "  SELECT id_alarm, name_sensor, min_ust, max_ust, deistv FROM Sensors_alarm where id_type= " + idT + " order by id_alarm";
            //  MessageBox.Show(comm);
            using (SqlConnection connection = new SqlConnection(connection_String))
            {
                connection.Open();
                FillSQLCommand(connection, comm, ref dsSens);
            }

            try
            {
                for (int i = 0; i < dsSens.Tables[0].Rows.Count; i++)
                {
                    FloatToTwoInt(Single.Parse(dsSens.Tables[0].Rows[i]["min_ust"].ToString()), ref SensMin[2 * i], ref SensMin[2 * i + 1]);
                    FloatToTwoInt(Single.Parse(dsSens.Tables[0].Rows[i]["max_ust"].ToString()), ref SensMax[2 * i], ref SensMax[2 * i + 1]);
                    SensDeistv[i] = short.Parse(dsSens.Tables[0].Rows[i]["deistv"].ToString());

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        */
        #endregion

        //Метод для записи шагов испытания для левой стороны
        #region Write Steps
        /*
        private void WriteSteps(int idTest, ref short[] Steps1,ref int count)

        {
           // int size = 16;//кол-во регистров R0-R15
                          //count  суммарное количество параметров в массиве кол-во параметров в строке*количество столбцов
            int size = 20;//кол-во регистров R2000-R2019
            int rezerv = 0;

            DataSet dsStep = new DataSet();
            dsStep.Clear();
            string comm = "SELECT * FROM Test_steps WHERE (id_isp = '" + idTest + "') ORDER BY id_steps";
            //"select * from step where [idTest]=" + idTest + " order by numberStep";

            //  MessageBox.Show(comm);
            using (SqlConnection connection = new SqlConnection(connection_String))
            {
                connection.Open();
                FillSQLCommand(connection, comm, ref dsStep);
            }

            try
            {
                count = (size) * dsStep.Tables[0].Rows.Count;
                for (int i = 0; i < dsStep.Tables[0].Rows.Count; i++)
                {

          
                    FloatToTwoInt(Single.Parse(dsStep.Tables[0].Rows[i]["deist_summ"].ToString()), ref Steps1[size * i], ref Steps1[size * i + 1]);
                    FloatToTwoInt(Single.Parse(dsStep.Tables[0].Rows[i]["distance"].ToString()), ref Steps1[size * i + 2], ref Steps1[size * i + 3]);
                    // FloatToTwoInt(Single.Parse(dsStep.Tables[0].Rows[i]["nagr"].ToString()), ref Steps1[size * i + 4], ref Steps1[size * i + 5]);

                    FloatToTwoInt((Single.Parse(dsStep.Tables[0].Rows[i]["nagr"].ToString()) / 10), ref Steps1[size * i + 4], ref Steps1[size * i + 5]);
                    FloatToTwoInt(Single.Parse(dsStep.Tables[0].Rows[i]["zapis_summ"].ToString()), ref Steps1[size * i + 6], ref Steps1[size * i + 7]);


                    Steps1[size * i + 8] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["deistvie"]);
                    Steps1[size * i + 9] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["speed"]);
                    Steps1[size * i + 10] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["zapis"]);
                    // Steps1[size * i + 11] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["ramp_razg"]);

                    Steps1[size * i + 11] = Convert.ToInt16((Single.Parse(dsStep.Tables[0].Rows[i]["ramp_razg"].ToString())) * 10);
                    //   Steps1[size * i + 12] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["ramp_nagr"]);
                    Steps1[size * i + 12] = Convert.ToInt16((Single.Parse(dsStep.Tables[0].Rows[i]["ramp_nagr"].ToString())) * 10);


                    Steps1[size * i + 13] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["num_disk"]);
                    //  Steps1[size * i + 14] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["temp"]);

                    Steps1[size * i + 14] = Convert.ToInt16((Single.Parse(dsStep.Tables[0].Rows[i]["temp"].ToString())) * 10);
                    Steps1[size * i + 15] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["diametr"]);

                    //Заполнение резервных регистров нулями
                    Steps1[size * i + 16] = Convert.ToInt16(rezerv.ToString());
                    Steps1[size * i + 17] = Convert.ToInt16(rezerv.ToString());
                    Steps1[size * i + 18] = Convert.ToInt16(rezerv.ToString());
                    Steps1[size * i + 19] = Convert.ToInt16(rezerv.ToString());


                    #region Commented out old code
                    //  Steps2[i] = short.Parse(dsStep.Tables[0].Rows[i]["deistvie"].ToString());
                    //     FloatToTwoInt(Single.Parse((dsStep.Tables[0].Rows[i]["deist_summ"]).ToString()), ref Steps1[2 * i], ref Steps1[2 * i + 1]);
                    //Stepsint[size * i] = Convert.ToInt32(dsStep.Tables[0].Rows[i]["Ramp_U"]);
                    //Steps[size * i] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["numberStep"]);
                    //Steps[size * i + 1] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["id_deistv"]);
                    //Steps[size * i + 2] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["stabilParamVal"]);
                    //Steps[size * i + 3] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["ramp"]);
                    //Steps[size * i + 4] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["dlit"]);
                    //Steps[size * i + 5] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["record"]);
                    //Steps[size * i + 6] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["period"]);
                    //Steps[size * i + 7] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["waitSensor"]);
                    //Steps[size * i + 8] = Convert.ToInt16(Math.Round(Convert.ToDouble(dsStep.Tables[0].Rows[i]["waitSensorVal"]), 0));
                    //Steps[size * i + 9] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["stabilTime"]);
                    //Steps[size * i + 10] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["otklon"]);
                    //Steps[size * i + 11] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["gtp"]);
                    //Steps[size * i + 12] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["gtpCount"]);
                    //Steps[size * i + 13] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["waitOper"]);
              
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + " step");
            }
        }
        */
        #endregion


        //Метод для записи шагов испытания для правой стороны
        #region WriteSteps2
        /*
        private void WriteSteps2(int idTest, ref short[] Steps1, ref int count)

        {
            //int size = 16;//кол-во регистров R0-R15
            int size = 20;//кол-во регистров R2000-R2019
            int rezerv = 0;
            //count  суммарное количество параметров в массиве кол-во параметров в строке*количество столбцов

            DataSet dsStep = new DataSet();
            dsStep.Clear();
            string comm = "SELECT * FROM Test_steps WHERE (id_isp = '" + idTest + "') ORDER BY id_steps";
            //"select * from step where [idTest]=" + idTest + " order by numberStep";

            //   MessageBox.Show(comm);
            using (SqlConnection connection = new SqlConnection(connection_String))
            {
                connection.Open();
                FillSQLCommand(connection, comm, ref dsStep);
            }

            try
            {
                count = (size) * dsStep.Tables[0].Rows.Count;
                for (int i = 0; i < dsStep.Tables[0].Rows.Count; i++)
                {

                    FloatToTwoInt(Single.Parse(dsStep.Tables[0].Rows[i]["deist_summ"].ToString()), ref Steps1[size * i], ref Steps1[size * i + 1]);
                    FloatToTwoInt(Single.Parse(dsStep.Tables[0].Rows[i]["distance"].ToString()), ref Steps1[size * i + 2], ref Steps1[size * i + 3]);
                    // FloatToTwoInt(Single.Parse(dsStep.Tables[0].Rows[i]["nagr"].ToString()), ref Steps1[size * i + 4], ref Steps1[size * i + 5]);

                    FloatToTwoInt((Single.Parse(dsStep.Tables[0].Rows[i]["nagr"].ToString()) / 10), ref Steps1[size * i + 4], ref Steps1[size * i + 5]);
                    FloatToTwoInt(Single.Parse(dsStep.Tables[0].Rows[i]["zapis_summ"].ToString()), ref Steps1[size * i + 6], ref Steps1[size * i + 7]);


                    Steps1[size * i + 8] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["deistvie"]);
                    Steps1[size * i + 9] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["speed"]);
                    Steps1[size * i + 10] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["zapis"]);
                    // Steps1[size * i + 11] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["ramp_razg"]);

                    Steps1[size * i + 11] = Convert.ToInt16((Single.Parse(dsStep.Tables[0].Rows[i]["ramp_razg"].ToString())) * 10);
                    //   Steps1[size * i + 12] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["ramp_nagr"]);
                    Steps1[size * i + 12] = Convert.ToInt16((Single.Parse(dsStep.Tables[0].Rows[i]["ramp_nagr"].ToString())) * 10);


                    Steps1[size * i + 13] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["num_disk"]);
                    //  Steps1[size * i + 14] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["temp"]);

                    Steps1[size * i + 14] = Convert.ToInt16((Single.Parse(dsStep.Tables[0].Rows[i]["temp"].ToString())) * 10);
                    Steps1[size * i + 15] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["diametr"]);
                    //Заполняем регистр R2016 данными для Г5 из столбца rezerv_1
                    Steps1[size * i + 16] = Convert.ToInt16(dsStep.Tables[0].Rows[i]["rezerv_1"]);

                    //Заполнение резервных регистров нулями
                    //    Steps1[size * i + 16] = Convert.ToInt16(rezerv.ToString());

                    Steps1[size * i + 17] = Convert.ToInt16(rezerv.ToString());
                    Steps1[size * i + 18] = Convert.ToInt16(rezerv.ToString());
                    Steps1[size * i + 19] = Convert.ToInt16(rezerv.ToString());

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " step");
            }
        }
        */
        #endregion

        #region Write Configurashion
        /*
        private void WriteConfig(int idConfig, int idDopConfig, ref short[] Config)
        {
            DataSet dsConfig = new DataSet();
            dsConfig.Clear();
            DataSet dsDopConfig = new DataSet();
            dsDopConfig.Clear();

            string comm = "select * from [SALEO_BD].[dbo].[StandParam] " +
                "where [id]=" + idConfig;
            string comm1 = "select * from [SALEO_BD].[dbo].[AddStandParam] " +
                "where [id]=" + idDopConfig;
            try
            {
                using (SqlConnection connection = new SqlConnection(connection_String))
                {
                    connection.Open();
                    //FillSQLCommand(connection, comm, ref dsConfig);
                    FillSQLCommand(connection, comm1, ref dsDopConfig);
                }

                Config[0] = short.Parse(dsDopConfig.Tables[0].Rows[0]["max_int_m1"].ToString());
                Config[1] = short.Parse(dsDopConfig.Tables[0].Rows[0]["max_int_m2"].ToString());
                Config[2] = short.Parse(dsDopConfig.Tables[0].Rows[0]["maxst_m"].ToString());
                Config[3] = short.Parse(dsDopConfig.Tables[0].Rows[0]["maxst_n"].ToString());
                Config[4] = short.Parse(dsDopConfig.Tables[0].Rows[0]["maxst_t"].ToString());
                Config[5] = short.Parse(dsDopConfig.Tables[0].Rows[0]["maxst_p"].ToString());
                Config[6] = short.Parse(dsDopConfig.Tables[0].Rows[0]["maxst_q"].ToString());
                Config[7] = short.Parse(dsDopConfig.Tables[0].Rows[0]["ns_order"].ToString());
                Config[8] = short.Parse(dsDopConfig.Tables[0].Rows[0]["ns_tormoz_val"].ToString());
                Config[9] = short.Parse(dsDopConfig.Tables[0].Rows[0]["ns_privod_val"].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " write config");
            }

        }
        */
        #endregion

        #region Write Calibration
        /*
        private void WriteCalibr(ref short[] CalibrMass)
        {
            try
            {
                DataSet dsChan1 = new DataSet();
                dsChan1.Clear();
                DataSet dsChan2 = new DataSet();
                dsChan2.Clear();

                string comm = "SELECT * FROM  Calibr1 order by id";

                string comm1 = "SELECT * FROM Calibr2 order by point";

                using (SqlConnection connection = new SqlConnection(connection_String))
                {
                    connection.Open();
                    FillSQLCommand(connection, comm, ref dsChan1);
                    FillSQLCommand(connection, comm1, ref dsChan2);
                }

                for (int i = 0; i < 6; i++)
                {
                    CalibrMass[8 * i] = short.Parse(Math.Round((Single.Parse(dsChan1.Tables[0].Rows[i]["downLim"].ToString()) * 1000)).ToString());
                    CalibrMass[8 * i + 1] = short.Parse(Math.Round((Single.Parse(dsChan1.Tables[0].Rows[i]["upLim"].ToString()) * 1000)).ToString());
                    CalibrMass[8 * i + 2] = short.Parse(Math.Round((Single.Parse(dsChan1.Tables[0].Rows[i]["valDowm"].ToString()) * 100)).ToString());
                    CalibrMass[8 * i + 3] = short.Parse(Math.Round((Single.Parse(dsChan1.Tables[0].Rows[i]["valUp"].ToString()) * 100)).ToString());
                    CalibrMass[8 * i + 4] = short.Parse(Math.Round((Single.Parse(dsChan1.Tables[0].Rows[i]["zero"].ToString()) * 1000)).ToString());
                    CalibrMass[8 * i + 5] = short.Parse(dsChan1.Tables[0].Rows[i]["onOff"].ToString());
                    CalibrMass[8 * i + 6] = short.Parse(Math.Round((Single.Parse(dsChan1.Tables[0].Rows[i]["obriv"].ToString()) * 1000)).ToString());
                    CalibrMass[8 * i + 7] = short.Parse(Math.Round((Single.Parse(dsChan1.Tables[0].Rows[i]["kz"].ToString()) * 1000)).ToString());
                }
                CalibrMass[48] = 0;

                for (int i = 6; i < 10; i++)
                {
                    CalibrMass[43 + i] = short.Parse(dsChan1.Tables[0].Rows[i]["onOff"].ToString());
                }

                CalibrMass[53] = 0;


                CalibrMass[54] = short.Parse(Math.Round((Single.Parse(dsChan1.Tables[0].Rows[10]["downLim"].ToString()) * 1000)).ToString());
                CalibrMass[55] = short.Parse(Math.Round((Single.Parse(dsChan1.Tables[0].Rows[10]["upLim"].ToString()) * 1000)).ToString());
                for (int i = 0; i < 13; i++)
                {
                    CalibrMass[56 + i * 2] =
                        short.Parse(Math.Round((Single.Parse(dsChan2.Tables[0].Rows[i]["acp"].ToString()) * 1000)).ToString());

                    CalibrMass[56 + i * 2 + 1] =
                        short.Parse(Math.Round((Single.Parse(dsChan2.Tables[0].Rows[i]["moment"].ToString()) * 10)).ToString());
                }

                CalibrMass[82] = short.Parse(Math.Round((Single.Parse(dsChan1.Tables[0].Rows[11]["downLim"].ToString()) * 1000)).ToString());
                CalibrMass[83] = short.Parse(Math.Round((Single.Parse(dsChan1.Tables[0].Rows[11]["upLim"].ToString()) * 1000)).ToString());
                for (int i = 13; i < 26; i++)
                {
                    CalibrMass[58 + i * 2] =
                        short.Parse(Math.Round((Single.Parse(dsChan2.Tables[0].Rows[i]["acp"].ToString()) * 1000)).ToString());

                    CalibrMass[58 + i * 2 + 1] =
                        short.Parse(Math.Round((Single.Parse(dsChan2.Tables[0].Rows[i]["moment"].ToString()) * 10)).ToString());
                }


                messInitCalibr = 0;
            }
            catch (Exception ex)
            {
                if (messInitCalibr == 0)
                {
                    messInitCalibr = 1;
                    MessageBox.Show(ex.Message + ". Проверте правильность задания значений на калибровочном экране!");
                }

            }

        }
        */
        #endregion

        #region READ params  Чтение данных из PLC в автоматическом режиме 
        //  private void ReadParams(short[] Params1, short[] Params2, short idProd1, short id_steps, short id_isp)
        private void ReadParams(short[] Params1, short idProd1, short id_steps, short id_isp)
        {
            string idMax = "0";
            
            #region Max id
            try
            {
                DataSet dSet = new DataSet();
                dSet.Clear();
                using (SqlConnection connection = new SqlConnection(connection_String))
                {
                    connection.Open();
                    dSet.Clear();
                    FillSQLCommand(connection, "SELECT MAX(id) as m FROM arhiv1", ref dSet);
                    if (dSet.Tables[0].Rows.Count != 0)
                    {
                        idMax = dSet.Tables[0].Rows[0]["m"].ToString();
                    }
                    else
                    {
                        idMax = "0";
                    }
                      MessageBox.Show("Метод ReadParams test");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " нахождение макс id");
            }
            #endregion
            try
            {

                #region old string conn
                //string comm = "INSERT INTO arhiv1(id_tire, id_isp, id_steps,dat, p0, p1, p2, p3, p4, p5, p6, p7, p8, p12, p9, p10, " +
                //  "p11,p13_rezerv,p14_rezerv, id) VALUES('" + idProd1.ToString() + "','" + id_isp.ToString() + "','" +
                //  id_steps.ToString() + "','" + DateTime.Now.ToString("dd.MM.yyyy HH: mm:ss") +
                //  // "','" +
                //  "','0','" +
                //  TwoIntToFloat(Params1[0], Params1[1]) + "','" +
                //  TwoIntToFloat(Params1[2], Params1[3]) + "','" +
                //  TwoIntToFloat(Params1[4], Params1[5]) + "','" +
                //  TwoIntToFloat(Params1[6], Params1[7]) + "','" +
                //  TwoIntToFloat(Params1[8], Params1[9]) + "','" +
                //  TwoIntToFloat(Params1[10], Params1[11]) + "','" +
                //  TwoIntToFloat(Params1[12], Params1[13]) + "','" +
                //  TwoIntToFloat(Params1[14], Params1[15]) + "','" +
                //  TwoIntToFloat(Params1[16], Params1[17]) + "','" +
                //  // TwoIntToFloat(Params1[18], Params1[19]) + "','" +

                //  TwoIntToFloat(Params2[0], Params2[1]) + "','" +
                //  TwoIntToFloat(Params2[2], Params2[3]) + "','" +
                //  TwoIntToFloat(Params2[4], Params2[5]) + "','" +
                //  "0" + "','" +
                //  "0" + "','" +
                //  idMax + "')";
                #endregion

                //  MessageBox.Show("Метод ReadParams  test 1");

                string conn = " INSERT INTO arhiv1(id_obj_name, id_isp, id_steps, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14," +
                            " p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25,p26, p27, p28, p29, p30_rez, p31_rez, p32_rez, p33_rez, id)" +
                            " VALUES('" + 
                            idProd1.ToString() + "','" +
                            id_isp.ToString() + "','" +
                            id_steps.ToString() + "','" +
                            TwoIntToFloat(Params1[0], Params1[1]) + "','" +
                            TwoIntToFloat(Params1[2], Params1[3]) + "','" +
                            TwoIntToFloat(Params1[4], Params1[5]) + "','" +
                            TwoIntToFloat(Params1[6], Params1[7]) + "','" +
                            TwoIntToFloat(Params1[8], Params1[9]) + "','" +
                            TwoIntToFloat(Params1[10], Params1[11]) + "','" +
                            TwoIntToFloat(Params1[12], Params1[13]) + "','" +
                            TwoIntToFloat(Params1[14], Params1[15]) + "','" +
                            TwoIntToFloat(Params1[16], Params1[17]) + "','" +
                            TwoIntToFloat(Params1[18], Params1[19]) + "','" +
                            TwoIntToFloat(Params1[20], Params1[21]) + "','" +
                            TwoIntToFloat(Params1[22], Params1[23]) + "','" +
                            TwoIntToFloat(Params1[24], Params1[25]) + "','" +
                            TwoIntToFloat(Params1[26], Params1[27]) + "','" +
                            TwoIntToFloat(Params1[28], Params1[29]) + "','" +
                            TwoIntToFloat(Params1[30], Params1[31]) + "','" +
                            TwoIntToFloat(Params1[32], Params1[33]) + "','" +
                            TwoIntToFloat(Params1[34], Params1[35]) + "','" +
                            TwoIntToFloat(Params1[36], Params1[37]) + "','" +
                            TwoIntToFloat(Params1[38], Params1[39]) + "','" +
                            TwoIntToFloat(Params1[40], Params1[41]) + "','" +
                            TwoIntToFloat(Params1[42], Params1[43]) + "','" +
                            TwoIntToFloat(Params1[44], Params1[45]) + "','" +
                            TwoIntToFloat(Params1[46], Params1[47]) + "','" +
                            TwoIntToFloat(Params1[48], Params1[49]) + "','" +
                            TwoIntToFloat(Params1[50], Params1[51]) + "','" +
                            TwoIntToFloat(Params1[52], Params1[53]) + "','" +
                            TwoIntToFloat(Params1[54], Params1[55]) + "','" +
                            TwoIntToFloat(Params1[56], Params1[57]) + "','" +
                            TwoIntToFloat(Params1[58], Params1[59]) + "','" +
                            TwoIntToFloat(Params1[60], Params1[61]) + "','" +
                            TwoIntToFloat(Params1[62], Params1[63]) + "','" +
                            TwoIntToFloat(Params1[64], Params1[65]) + "','" + 
                            TwoIntToFloat(Params1[66], Params1[67]) + "','" +
                            idMax + "')";

                //MessageBox.Show(conn);
                MessageBox.Show("readParams");
                MessageBox.Show("readParams1");
                                       

                using (SqlConnection connection = new SqlConnection(connection_String))
                {
                    connection.Open();
                    UpdateSQLCommand(connection, conn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ReadParams");
            }
        }
        #endregion

        #region READ params  Чтение данных из PLC в автоматическом режиме правая сторона
        /*
        private void ReadParams2(short[] Params1, short[] Params2, short idProd1, short id_steps, short id_isp)
        {

            string idMax = "0";

            #region Max id
            try
            {

                DataSet dSet = new DataSet();
                dSet.Clear();
                using (SqlConnection connection = new SqlConnection(connection_String))
                {
                    connection.Open();
                    dSet.Clear();
                    FillSQLCommand(connection, "SELECT MAX(id) as m FROM arhiv2", ref dSet);
                    if (dSet.Tables[0].Rows.Count != 0)
                    {
                        idMax = dSet.Tables[0].Rows[0]["m"].ToString();
                    }
                    else
                    {
                        idMax = "0";
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " нахождение макс id");
            }
          
            #endregion

            try
            {
                #region Old code comment
                //string comm = "INSERT INTO arhiv2(id_tire, id_isp, id_steps,dat, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, " +
                //  "p11,p12_rezerv, p13_rezerv, id) VALUES('" + idProd1.ToString() + "','" + id_isp.ToString() + "','" +
                //  id_steps.ToString() + "','" + DateTime.Now.ToString("dd.MM.yyyy HH: mm:ss") +
                //  "','" + TwoIntToFloat(Params1[0], Params1[1]) + "','" +
                //  TwoIntToFloat(Params1[2], Params1[3]) + "','" +
                //  TwoIntToFloat(Params1[4], Params1[5]) + "','" +
                //  TwoIntToFloat(Params1[6], Params1[7]) + "','" +
                //  TwoIntToFloat(Params1[8], Params1[9]) + "','" +
                //  TwoIntToFloat(Params1[10], Params1[11]) + "','" +
                //  TwoIntToFloat(Params1[12], Params1[13]) + "','" +
                //  TwoIntToFloat(Params1[14], Params1[15]) + "','" +
                //  TwoIntToFloat(Params2[0], Params2[1]) + "','" +
                //  TwoIntToFloat(Params2[2], Params2[3]) + "','" +
                //  TwoIntToFloat(Params2[4], Params2[5]) + "','" +
                //  "0" + "','" +
                //  "0" + "','" +
                //  idMax + "')";
                #endregion

                string comm = "INSERT INTO arhiv2(id_tire, id_isp, id_steps,dat, p0, p1, p2, p3, p4, p5, p6, p7, p8,p12, p9, p10, " +
                 "p11,p13_rezerv,p14_rezerv, id) VALUES('" + idProd1.ToString() + "','" + id_isp.ToString() + "','" +
                 id_steps.ToString() + "','" + DateTime.Now.ToString("dd.MM.yyyy HH: mm:ss") +
                 //"','0','" +
                 "','" +
                 TwoIntToFloat(Params1[0], Params1[1]) + "','" +
                 TwoIntToFloat(Params1[2], Params1[3]) + "','" +
                 TwoIntToFloat(Params1[4], Params1[5]) + "','" +
                 TwoIntToFloat(Params1[6], Params1[7]) + "','" +
                 TwoIntToFloat(Params1[8], Params1[9]) + "','" +
                 TwoIntToFloat(Params1[10], Params1[11]) + "','" +
                 TwoIntToFloat(Params1[12], Params1[13]) + "','" +
                 TwoIntToFloat(Params1[14], Params1[15]) + "','" +
                 TwoIntToFloat(Params1[16], Params1[17]) + "','" +
                 TwoIntToFloat(Params1[18], Params1[19]) + "','" +

                 TwoIntToFloat(Params2[0], Params2[1]) + "','" +
                 TwoIntToFloat(Params2[2], Params2[3]) + "','" +
                 TwoIntToFloat(Params2[4], Params2[5]) + "','" +
                 "0" + "','" +
                 "0" + "','" +
                 idMax + "')";

               // MessageBox.Show(comm);

                using (SqlConnection connection = new SqlConnection(connection_String))
                {
                    connection.Open();
                    UpdateSQLCommand(connection, comm);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "readParamAutoRight");
            }
        }
        */
        #endregion

        #region READ params in man Чтение данных из PLC в ручном режиме левая сторона
        /*
        private void ReadManParamLeft(short[] Params1, short[] Params2, short idProd1)
        {
            string idMax = "0";
            #region Max id
            try
            {
                DataSet dSet = new DataSet();
                dSet.Clear();
                using (SqlConnection connection = new SqlConnection(connection_String))
                {
                    connection.Open();
                    dSet.Clear();
                    FillSQLCommand(connection, "SELECT MAX(id) as m FROM arhiv1", ref dSet);
                    if (dSet.Tables[0].Rows.Count != 0)
                    {
                        idMax = dSet.Tables[0].Rows[0]["m"].ToString();
                    }
                    else
                    {
                        idMax = "0";
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " нахождение макс id");
            }
           
            #endregion

            try
            {
                //Перед добавление КСК
                //string comm = "INSERT INTO arhiv1(id_tire, id_isp, id_steps,dat, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, " +
                //  "p11,p12_rezerv, p13_rezerv, id) VALUES('" + idProd1.ToString() + "','400','300','" + DateTime.Now.ToString("dd.MM.yyyy HH: mm:ss") +
                //  "','" + TwoIntToFloat(Params1[0], Params1[1]) + "','" +
                //  TwoIntToFloat(Params1[2], Params1[3]) + "','" +
                //  TwoIntToFloat(Params1[4], Params1[5]) + "','" +
                //  TwoIntToFloat(Params1[6], Params1[7]) + "','" +
                //  TwoIntToFloat(Params1[8], Params1[9]) + "','" +
                //  TwoIntToFloat(Params1[10], Params1[11]) + "','" +
                //  TwoIntToFloat(Params1[12], Params1[13]) + "','" +
                //  TwoIntToFloat(Params1[14], Params1[15]) + "','" +
                //  TwoIntToFloat(Params2[0], Params2[1]) + "','" +
                //  TwoIntToFloat(Params2[2], Params2[3]) + "','" +
                //  TwoIntToFloat(Params2[4], Params2[5]) + "','" +
                //  "0" + "','" +
                //  "0" + "','" +
                //  idMax + "')";

                //После ввода Параметра КСК и КОДА ошибки

                string comm = "INSERT INTO arhiv1(id_tire, id_isp, id_steps,dat, p0, p1, p2, p3, p4, p5, p6, p7, p8,p12, p9, p10, " +
                 "p11,p13_rezerv,p14_rezerv, id) VALUES('" + idProd1.ToString() + "','400','300','" + DateTime.Now.ToString("dd.MM.yyyy HH: mm:ss") +
                 "','0','" +
                 TwoIntToFloat(Params1[0], Params1[1]) + "','" +
                 TwoIntToFloat(Params1[2], Params1[3]) + "','" +
                 TwoIntToFloat(Params1[4], Params1[5]) + "','" +
                 TwoIntToFloat(Params1[6], Params1[7]) + "','" +
                 TwoIntToFloat(Params1[8], Params1[9]) + "','" +
                 TwoIntToFloat(Params1[10], Params1[11]) + "','" +
                 TwoIntToFloat(Params1[12], Params1[13]) + "','" +
                 TwoIntToFloat(Params1[14], Params1[15]) + "','" +
                 TwoIntToFloat(Params1[16], Params1[17]) + "','" +
              //   TwoIntToFloat(Params1[18], Params1[19]) + "','" +
               
                 TwoIntToFloat(Params2[0], Params2[1]) + "','" +
                 TwoIntToFloat(Params2[2], Params2[3]) + "','" +
                 TwoIntToFloat(Params2[4], Params2[5]) + "','" +
                 "0" + "','" +
                 "0" + "','" +
                 idMax + "')";

                MessageBox.Show(comm);

                #region Comment Old Code
                //string comm = "INSERT INTO arhiv(id_gen, id_isp, id_steps,dat, p1, p2, p3, p4, p5, p6, p7, p8_rezerv, p9, p10, " +
                //    "p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, id) VALUES('" + idProd1.ToString() + "','400','300','" + DateTime.Now.ToString("dd.MM.yyyy HH: mm:ss") +
                //    "','" + TwoIntToFloat(Params1[0], Params1[1]) + "','" +
                //    TwoIntToFloat(Params1[2], Params1[3]) + "','" +
                //    TwoIntToFloat(Params1[4], Params1[5]) + "','" +
                //    TwoIntToFloat(Params1[6], Params1[7]) + "','" +
                //    TwoIntToFloat(Params1[8], Params1[9]) + "','" +
                //    TwoIntToFloat(Params1[10], Params1[11]) + "','" +
                //    TwoIntToFloat(Params1[12], Params1[13]) + "','" +
                //    TwoIntToFloat(Params1[14], Params1[15]) + "','" +
                //    TwoIntToFloat(Params1[16], Params1[17]) + "','" +
                //    TwoIntToFloat(Params1[18], Params1[19]) + "','" +
                //    TwoIntToFloat(Params1[20], Params1[21]) + "','" +
                //    TwoIntToFloat(Params1[22], Params1[23]) + "','" +
                //    TwoIntToFloat(Params1[24], Params1[25]) + "','" +
                //    TwoIntToFloat(Params1[26], Params1[27]) + "','" +
                //    TwoIntToFloat(Params2[0], Params2[1]) + "','" +
                //    TwoIntToFloat(Params2[2], Params2[3]) + "','" +
                //    TwoIntToFloat(Params2[4], Params2[5]) + "','" +
                //    TwoIntToFloat(Params2[6], Params2[7]) + "','" +
                //    TwoIntToFloat(Params2[8], Params2[9]) + "','" +
                //    TwoIntToFloat(Params2[10], Params2[11]) + "','" +
                //    idMax + "')";

                #endregion

                //      MessageBox.Show(comm);
                using (SqlConnection connection = new SqlConnection(connection_String))
                {
                    connection.Open();
                    UpdateSQLCommand(connection, comm);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "readParamMan");
            }

        }
        */
        #endregion

        #region READ params in man Чтение данных из PLC в ручном режиме правая сторона
        /*
        private void ReadManParamRight(short[] Params1, short[] Params2, short idProd1)
        {
            string idMax = "0";
            #region Max id
            try
            {
                DataSet dSet = new DataSet();
                dSet.Clear();
                using (SqlConnection connection = new SqlConnection(connection_String))
                {
                    connection.Open();
                    dSet.Clear();
                    FillSQLCommand(connection, "SELECT MAX(id) as m FROM arhiv2", ref dSet);
                    if (dSet.Tables[0].Rows.Count != 0)
                    {
                        idMax = dSet.Tables[0].Rows[0]["m"].ToString();
                    }
                    else
                    {
                        idMax = "0";
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " нахождение макс id");
            }

            #endregion

            try
            {


                string comm = "INSERT INTO arhiv2(id_tire, id_isp, id_steps,dat, p0, p1, p2, p3, p4, p5, p6, p7, p8,p12, p9, p10, " +
                "p11,p13_rezerv,p14_rezerv, id) VALUES('" + idProd1.ToString() + "','400','300','" + DateTime.Now.ToString("dd.MM.yyyy HH: mm:ss") +
                //"','0','" +
                  "','" +
                TwoIntToFloat(Params1[0], Params1[1]) + "','" +
                TwoIntToFloat(Params1[2], Params1[3]) + "','" +
                TwoIntToFloat(Params1[4], Params1[5]) + "','" +
                TwoIntToFloat(Params1[6], Params1[7]) + "','" +
                TwoIntToFloat(Params1[8], Params1[9]) + "','" +
                TwoIntToFloat(Params1[10], Params1[11]) + "','" +
                TwoIntToFloat(Params1[12], Params1[13]) + "','" +
                TwoIntToFloat(Params1[14], Params1[15]) + "','" +
                TwoIntToFloat(Params1[16], Params1[17]) + "','" +
                TwoIntToFloat(Params1[18], Params1[19]) + "','" +

                TwoIntToFloat(Params2[0], Params2[1]) + "','" +
                TwoIntToFloat(Params2[2], Params2[3]) + "','" +
                TwoIntToFloat(Params2[4], Params2[5]) + "','" +
                "0" + "','" +
                "0" + "','" +
                idMax + "')";

                MessageBox.Show(comm);

                #region Comment Old Code
                //string comm = "INSERT INTO arhiv(id_gen, id_isp, id_steps,dat, p1, p2, p3, p4, p5, p6, p7, p8_rezerv, p9, p10, " +
                //    "p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, id) VALUES('" + idProd1.ToString() + "','400','300','" + DateTime.Now.ToString("dd.MM.yyyy HH: mm:ss") +
                //    "','" + TwoIntToFloat(Params1[0], Params1[1]) + "','" +
                //    TwoIntToFloat(Params1[2], Params1[3]) + "','" +
                //    TwoIntToFloat(Params1[4], Params1[5]) + "','" +
                //    TwoIntToFloat(Params1[6], Params1[7]) + "','" +
                //    TwoIntToFloat(Params1[8], Params1[9]) + "','" +
                //    TwoIntToFloat(Params1[10], Params1[11]) + "','" +
                //    TwoIntToFloat(Params1[12], Params1[13]) + "','" +
                //    TwoIntToFloat(Params1[14], Params1[15]) + "','" +
                //    TwoIntToFloat(Params1[16], Params1[17]) + "','" +
                //    TwoIntToFloat(Params1[18], Params1[19]) + "','" +
                //    TwoIntToFloat(Params1[20], Params1[21]) + "','" +
                //    TwoIntToFloat(Params1[22], Params1[23]) + "','" +
                //    TwoIntToFloat(Params1[24], Params1[25]) + "','" +
                //    TwoIntToFloat(Params1[26], Params1[27]) + "','" +
                //    TwoIntToFloat(Params2[0], Params2[1]) + "','" +
                //    TwoIntToFloat(Params2[2], Params2[3]) + "','" +
                //    TwoIntToFloat(Params2[4], Params2[5]) + "','" +
                //    TwoIntToFloat(Params2[6], Params2[7]) + "','" +
                //    TwoIntToFloat(Params2[8], Params2[9]) + "','" +
                //    TwoIntToFloat(Params2[10], Params2[11]) + "','" +
                //    idMax + "')";

                #endregion

                //      MessageBox.Show(comm);
                using (SqlConnection connection = new SqlConnection(connection_String))
                {
                    connection.Open();
                    UpdateSQLCommand(connection, comm);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "readParamMan");
            }

        }
        */
        #endregion

        #region DecodeTime
        private DateTime DecodeTime(int Year, int Month, int Day, int H, int M, int S, int ms)
        {
            DateTime timeRec = new DateTime((Year + 2000), Month, Day, H, M, S, ms);

            return timeRec;
        }
        #endregion

        #region time synchronization
        private void SynchronTime(string reg)
        {
            short[] timeSyn = new short[7];
            timeSyn[0] = (short)(DateTime.Now.Year - 2000);
            timeSyn[1] = (short)DateTime.Now.Month;
            timeSyn[2] = (short)DateTime.Now.Day;
            timeSyn[3] = (short)DateTime.Now.Hour;
            timeSyn[4] = (short)DateTime.Now.Minute;
            timeSyn[5] = (short)DateTime.Now.Second;
            timeSyn[6] = (short)DateTime.Now.Millisecond;

            try
            {
                MxComp.WriteDeviceBlock2(reg, 7, ref timeSyn[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " time synchron");
            }
        }
        #endregion

        #region SQLMetods
        private void UpdateSQLCommand(SqlConnection connection, string commandText)
        {

            try
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                command.ExecuteNonQuery();
            }
            catch (SqlException exp)
            {
                MessageBox.Show(exp.Message);
            }
        }


        private void FillSQLCommand(SqlConnection connection, string comm, ref DataSet dset)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(comm, connection);
                adapter.Fill(dset);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);

            }

        }
        #endregion

        #region Time
        private void timerTime_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToString();
        }
        #endregion

       
    }
}
