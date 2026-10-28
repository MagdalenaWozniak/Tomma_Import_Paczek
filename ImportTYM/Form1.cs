using cdn_api;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportPaczek
{
    public partial class Form1 : Form
    {
        private int XLWersjaAPI = 20232;
        private int XLTrybWsadowy = 1;
        private int XLWinieta = -1;
        private int XLUtworzWlasnaSesje = 1;
        private int sesja = -1;
        private string XLSQLServer = "10.33.21.10";
        private string XLSQLDatabase = "BONUS-2"; //"TOMMA-ERP";//"BONUS-2";//"TOMMA-ERP";//"ERPXL_PoloMarket"
        private string XLUser = "MACIEJ"; //
        private string XLPass = "NE!w0rk12";//"Dupa2003!";
        string connectionString = @"Data Source = COMARCHBD; Initial Catalog = ERPXL_BONUS_2; User ID = sa; Password=fr$g4i9sSa032V,_";//@"Data Source = COMARCHBD; Initial Catalog = ERPXL_TOMMA-ERP; User ID = sa; Password=fr$g4i9sSa032V,_";// @"Data Source = COMARCHBD; Initial Catalog = ERPXL_BONUS_2; User ID = sa; Password=fr$g4i9sSa032V,_";//@"Data Source = COMARCHBD; Initial Catalog = ERPXL_TOMMA-ERP; User ID = sa; Password=fr$g4i9sSa032V,_";
        string date1 = "1800-12-28";

        //@"Data Source = SQLSERVER; Initial Catalog = DomarXL; User ID=AdminS;Password=6,eF#W451,t6,D@";
         
        public Form1()
        {
            //this.connectionString = GetConnectionString();
            InitializeComponent();
            string selectRejestry = "SELECT KAR_Seria FROM cdn.Rejestry WHERE KAr_tYP=2 and KAR_Waluta='PLN'";
            List<string> rejestry2 = SelectListRowColumn(selectRejestry, "Rejestry");
            string[] stringRejestry = new string[rejestry2.Count];
            int i = 0;
            int selectedIndex = 0;
            foreach (string format in rejestry2)
            {
                stringRejestry[i] = format;
                if (format.Contains("domyślny"))
                    selectedIndex = i;
                i++;

            }
            rejestr.Items.AddRange(stringRejestry);
            if (i > 0)
                rejestr.SelectedIndex = selectedIndex;
        }


        void Button1_Click(object sender, EventArgs e)
        {
            #region excel5
            #region wybranie pliku
            Cursor.Current = Cursors.WaitCursor;
            if (tB1.Text == "nie wybrano pliku")
            {
                MessageBox.Show("Nie wybrano pliku arkusza.", "Komunikat",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string filePath = tB1.Text;
            DataTable dataTable;

           /* using (XlsxOrXlsbReadOrEdit excelFile = new XlsxOrXlsbReadOrEdit())
            {
                excelFile.Open("file.xlsx");
                excelFile.ActualSheetName = "sheet1";
                object[] row = null;
                while (excelFile.Read())
                {
                    if (row == null)
                    {
                        row = new object[excelFile.FieldCount];
                    }
                    excelFile.GetValues(row);
                }
            }*/

            
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
             {
                 IExcelDataReader reader;
                 reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

                 var conf = new ExcelDataSetConfiguration
                 {
                     ConfigureDataTable = _ => new ExcelDataTableConfiguration
                     {
                         UseHeaderRow = false//true
                     }
                 };

                 var dataSet = reader.AsDataSet(conf);
                 dataTable = dataSet.Tables[0];
             }






            #endregion

            #region Przypisywanie danych z datasetu do listy przyszłych pozycji.

            SprawdzamCzyArkuszNieJestPusty(dataTable);
            dodajDokumenty(dataTable);

            MessageBox.Show("Zakończono dodawanie paczki", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
            #endregion

            #endregion


        }
        #region Dodawanie dokumetów
        void dodajDokumenty(DataTable dataTable)
        {
            
            string maxnUMERqUERY= "SELECT ISNULL(MAX([PPN_Numer]) + 1,1) FROM [CDN].[PrzelewyNag] where PPN_Miesiac = "+ dataPicker.Value.Month +" and PPN_Rok = "+ dataPicker.Value.Year +" and " +
                                    " PPN_Rejestr = '"+ rejestr.SelectedItem.ToString() +"'";
            string numerRejestr = ConnectToSQL(connectionString, maxnUMERqUERY);

            string insertPaczka = "INSERT INTO [CDN].[PrzelewyNag]  ([PPN_Rejestr] ,[PPN_Rok],[PPN_Miesiac] ,[PPN_Numer] ,[PPN_Status] ,[PPN_DataDokumentu] ,[PPN_DataRealizacji] ,[PPN_OpeWystNumer] ,[PPN_OpeModyfNumer]" +
                                    " ,[PPN_OpeZatwNumer] ,[PPN_OpeRealizNumer] ,[PPN_RealizTS]  ,[PPN_URL] ,[PPN_Opis],[PPN_Uwagi]  ,[PPN_Aktywny]) VALUES  ('" + rejestr.SelectedItem.ToString() + "'," + dataPicker.Value.Year +
                                    "   ," + dataPicker.Value.Month + "  ," + numerRejestr + ",0 ," + GetClarionDate(dataPicker.Value) + "  , 0, 1, 1 , 0 , 0 , 0 , '' , '' , '' , 0)";
            InsertInto(insertPaczka, connectionString);
                


            
            pBar1.Visible = true;
            // Set Minimum to 1 to represent the first file being copied.
            pBar1.Minimum = 1;
            // Set Maximum to the total number of files to copy.
            pBar1.Maximum = dataTable.Rows.Count;
            // Set the initial value of the ProgressBar.
            pBar1.Value = 1;
            // Set the Step property to a value of 1 to represent each file being copied.
            pBar1.Step = 1;


            

            string IDpaczkiQuery = "SELECT PPN_ID  FROM[CDN].[PrzelewyNag] where PPN_Miesiac = " + dataPicker.Value.Month + " and PPN_Rok = " + dataPicker.Value.Year + " and " +
                                    " PPN_Rejestr = '" + rejestr.SelectedItem.ToString() + "' and PPN_Numer=" + numerRejestr;
            string IDpaczki = ConnectToSQL(connectionString, IDpaczkiQuery);

            int lpElem = 1;


            for (int i = 1; i < dataTable.Rows.Count; i++)
            {
                try
                {
                    pBar1.PerformStep();
                    if(i==dataTable.Rows.Count-1)
                    {
                        MessageBox.Show("i:"+i+"\nTraPalt Numer: " + Convert.ToString(dataTable.Rows[i][1]) + "\n" + "TraPlat Lp:" + Convert.ToString(dataTable.Rows[i][2]) +
                            "\nx: " + Convert.ToString(dataTable.Rows[i][16])
                            + "\nWaluta : " + Convert.ToString(dataTable.Rows[i][10])
                            + "\n reszta:" + Convert.ToString(dataTable.Rows[i][0]) + Convert.ToString(dataTable.Rows[i][3]) + Convert.ToString(dataTable.Rows[i][4]) + Convert.ToString(dataTable.Rows[i][5]) + Convert.ToString(dataTable.Rows[i][6])
                            + Convert.ToString(dataTable.Rows[i][7]) + Convert.ToString(dataTable.Rows[i][8]) + Convert.ToString(dataTable.Rows[i][9]) + Convert.ToString(dataTable.Rows[i][11]) + Convert.ToString(dataTable.Rows[i][12])
                            + Convert.ToString(dataTable.Rows[i][13]) + Convert.ToString(dataTable.Rows[i][14]) + Convert.ToString(dataTable.Rows[i][15]));
                    }
                    string gidTyp = "1521";
                    if(Convert.ToString(dataTable.Rows[i][3]).Contains("FZK"))
                    {
                        gidTyp = "1529";
                    }

                    string istniejeQuery = "if exists(SELECT * FROM [CDN].[Przelewy] join cdn.PrzelewyNag on PPN_ID=PPX_PPNID join cdn.PrzelewyElem on PPX_PPELp=PPE_PPELP and PPN_ID=PPE_PPNID  WHERE [PPX_TRPTyp]= "+gidTyp+" and [PPX_TRPFirma]=1176321  and [PPX_TRPNumer]=" + Convert.ToString(dataTable.Rows[i][1]) + " and [PPX_TRPLp]=" + Convert.ToString(dataTable.Rows[i][2]) + " and PPN_Status!=10 and PPE_Odrzucona!=1) SELECT 'TAK' else SELECT 'NIE'";
                    string istnieje = ConnectToSQL(connectionString,istniejeQuery);

                    string czyZgodneQuery = "if((SELECT KAR_Waluta from cdn.Rejestry where Kar_Seria='" + rejestr.SelectedItem.ToString() + "')=(SELECT TrP_Waluta from cdn.TraPlat where TrP_GIDNumer = " + Convert.ToString(dataTable.Rows[i][1]) + " and TrP_GIDTyp ="+gidTyp+" and TrP_GIDLp = " + Convert.ToString(dataTable.Rows[i][2]) + ") ) "
                                            + " SELECT 'TAK' else SELECT 'NIE'";
                    string zgodne = ConnectToSQL(connectionString, czyZgodneQuery);

                    string statusQuery = "SELECT TRP_Status FROM cdn.TraPlat WHERE TRP_GIDTyp ="+gidTyp+" and TrP_GIDNumer ="+ Convert.ToString(dataTable.Rows[i][1])+" and TrP_GIDLp = " + Convert.ToString(dataTable.Rows[i][2]);
                    string status = ConnectToSQL(connectionString, statusQuery);

                    string rozlQuery = "SELECT TRP_Rozliczona FROM cdn.TraPlat WHERE TRP_GIDTyp = "+gidTyp+" and TrP_GIDNumer = " + Convert.ToString(dataTable.Rows[i][1]) + " and TrP_GIDLp = " + Convert.ToString(dataTable.Rows[i][2]);
                    string rozl = ConnectToSQL(connectionString, rozlQuery);

                    if ((istnieje == "NIE" && status == "0" && rozl != "2") && zgodne == "TAK" && Convert.ToString(dataTable.Rows[i][10]).Trim()=="PLN" &&   (Convert.ToString(dataTable.Rows[i][16]).Trim() == "x" || Convert.ToString(dataTable.Rows[i][16]).Trim() == "X"))
                    {
                        string obcyQuery = "SELECT TrN_DokumentObcy From cdn.TraNag where TrN_GIDNumer= " + Convert.ToString(dataTable.Rows[i][1]) + " and TrN_GIDTyp="+gidTyp;
                        string obcy = ConnectToSQL(connectionString, obcyQuery);

                        string lpQuery = "SELECT ISNULL(max(PPE_Lp),0)  FROM cdn.PrzelewyElem";
                    int lp = Convert.ToInt32(ConnectToSQL(connectionString, lpQuery)) + 1;

                  //  MessageBox.Show("Ilość wierszy: "+dataTable.Rows.Count+" Ilość Kolumn: "+dataTable.Columns.Count, "Komunikat",
                    //      MessageBoxButtons.OK, MessageBoxIcon.Warning);


                     string insertElem = "INSERT INTO [CDN].[PrzelewyElem] ([PPE_PPNID] ,[PPE_PPELp] ,[PPE_Odrzucona]  ,[PPE_Lp] ,[PPE_GrpPPELp]  ,[PPE_Grupa]  ,[PPE_TytulWyrazenie]  ,[PPE_TytulWartosc]  ,[PPE_TytulSeparator] ,[PPE_TytulModyfikowany]  ,[PPE_PodzialGrupy]"
                                          + "  ,[PPE_PodzialIloscZnakow]    ,[PPE_ModyfOpeNumer]  ,[PPE_ModyfData]) VALUES (" + IDpaczki+ " , " + lpElem + " , 0, " + lpElem + ", 0, 0, '', '" + obcy+ "' , '', 0, 0, 0, 1, "+ GetClarionDate(dataPicker.Value)+")";

                     // MessageBox.Show("insertElem: " + insertElem, "Komunikat",
                         //   MessageBoxButtons.OK, MessageBoxIcon.Warning);

                      InsertInto(insertElem, connectionString);

                    string selectRejestr = "SELECT KAR_SEria FROM cdn.TraPlat join cdn.Rejestry on KAR_GIDNumer = TrP_KARNumer and KAR_GIDTyp = TrP_KARTyp and KAR_GIDFirma = TRP_KARFirma WHERE TrP_GIDTyp = "+gidTyp+" and TrP_GIDFirma = 1176321" +
                                             " and TrP_GIDNumer = "+ Convert.ToString(dataTable.Rows[i][1]);
                    string rejestrDok = ConnectToSQL(connectionString, selectRejestr);

                        if (rejestrDok.Trim() != rejestr.SelectedItem.ToString().Trim())
                        {
                            string selectGidRejestr = "SELECT KAr_gidnUMER FROM cdn.Rejestry where KAr_Seria='" + rejestr.SelectedItem.ToString() + "'";
                            string GidRejestr = ConnectToSQL(connectionString, selectGidRejestr);

                            string updateRejestr = "UPDATE [CDN].[TraPlat] SET [TrP_KARNumer] = " + GidRejestr + " WHERE TrP_GIDTyp = "+gidTyp+" and TrP_GIDFirma = 1176321 and TrP_GIDNumer = " + Convert.ToString(dataTable.Rows[i][1]);
                            Update(updateRejestr, connectionString);
                        }

                  











                    string insertQuery = "INSERT INTO [CDN].[Przelewy] ([PPX_TRPTyp] ,[PPX_TRPFirma]  ,[PPX_TRPNumer] ,[PPX_TRPLp] ,[PPX_KwotaAkc] ,[PPX_KwotaExp]  ,[PPX_ExpID] ,[PPX_ExpLin]  ,[PPX_Status] ,[PPX_PPNID] ,[PPX_PPELp])"
                                       + " VALUES ( "+gidTyp+", 1176321, "+Convert.ToString(dataTable.Rows[i][1])+" , "+ Convert.ToString(dataTable.Rows[i][2]) + ", " + Convert.ToString(dataTable.Rows[i][9]).Replace(",", ".") + " , " + Convert.ToString(dataTable.Rows[i][9]).Replace(",", ".") + 
                                       ", 0, 0, 0 , " + IDpaczki + ", " + lpElem + ")";

                     //  MessageBox.Show("insertQuery: " + insertQuery, "Komunikat",
                     //         MessageBoxButtons.OK, MessageBoxIcon.Warning);

                     InsertInto(insertQuery, connectionString);




                    lpElem++;

                    }
                    else if (istnieje == "TAK")
                        MessageBox.Show("Dokument " + Convert.ToString(dataTable.Rows[i][3]) + " jest już dodany do paczki", "Komunikat",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else if (status != "0")
                        MessageBox.Show("Dokument " + Convert.ToString(dataTable.Rows[i][3]) + " ma status inny niż 'Do wysłania'", "Komunikat",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else if (rozl == "2") 
                       MessageBox.Show("Dokument " + Convert.ToString(dataTable.Rows[i][3]) + " ma status 'Nie rozliczaj'", "Komunikat",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   // else
                      //  MessageBox.Show("Dokument " + Convert.ToString(dataTable.Rows[i][3]) + " ma niezgodną walutę z paczką " + IDpaczki, "Komunikat",
                       //     MessageBoxButtons.OK, MessageBoxIcon.Warning);



                }
                catch (Exception e)
                {
                    MessageBox.Show("Błąd dodawania dokumentu "+ Convert.ToString(dataTable.Rows[i][3]) + " do paczki "+IDpaczki+": " + e.Message, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


            }

         


        }

        public string Update(string sql, string connection_string)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    connection.Open();
                    adapter.UpdateCommand = new SqlCommand(sql, connection);
                    adapter.UpdateCommand.ExecuteNonQuery();
                    //MessageBox.Show("Row updated !! ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sql;
        }


        public List<string> SelectListRowColumn(string query, string srcTableName)
        {
            List<string> str = new List<string>();
            string sttr = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(query, connection);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, srcTableName);
                    foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                    {
                        foreach (DataColumn column in (InternalDataCollectionBase)dataSet.Tables[0].Columns)
                        {
                            str.Add(row[column].ToString());

                            //MessageBox.Show(column + ": " + row[column].ToString());
                        }
                       

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(sttr = sttr + "Bład SQL:" + ex.ToString() + "\n");
            }
            return str;
        }


        public static int GetClarionDate(DateTime date)
        {
            DateTime baseDate = DateTime.Parse("1801-01-01");
            System.TimeSpan ts = date - baseDate;
            int l = ts.Days + 4;
            return l;
        }



        public string InsertInto(string sql, string connection_string)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    connection.Open();
                    adapter.InsertCommand = new SqlCommand(sql, connection);
                    adapter.InsertCommand.ExecuteNonQuery();
                    //MessageBox.Show("Row inserted !! ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sql;
        }
        public string ConnectToSQL(string connection_string, string query)
        {
            string str = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(query, connection);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, "Query");
                    if (dataSet.Tables.Count == 0)
                    {
                        return "";
                    }
                    foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                    {
                        str = row[0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                str = str + "Bład SQL:" + ex.ToString() + "\n";
            }
            return str;
        }
        #endregion

        #region funkcje wybranie pliku
        void WybraniePliku()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog()
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                DefaultExt = "xlsx",
                Filter = "Pliki xlsx (*.xlsx)|*.xlsx|Pliki xls (*.xls)|*.xls|Wszystkie pliki (*.*)|*.*",
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tB1.Text = openFileDialog1.FileName;
                //MessageBox.Show(path);
            }
            return;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            WybraniePliku();
        }
        private void TextBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WybraniePliku();
        }
        #endregion

        #region funkcje pobranie danych z pliku
        bool SprawdzamCzyArkuszNieJestPusty(DataTable _dataTable)
        {
            if (_dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Wybrany arkusz pusty.", "Komunikat",
                MessageBoxButtons.OK, MessageBoxIcon.Error);//błąd
                return true;
            }
            return false;
        }
        #endregion

        private void title1_Click(object sender, EventArgs e)
        {

        }

    }
}

