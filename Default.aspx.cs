using NoelLevitz.ForecastPlus.Data.Models;
using NoelLevitz.ForecastPlus.Shared;
using NoelLevitz.Import;
using NoelLevitz.Import.Helpers;
using NoelLevitz.Import.Interfaces;
using NoelLevitz.Import.Validation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebDoImport
{

    /// <summary>
    /// This is used for testing the first main part of the import system from behind a web app.
    /// This does everything prior to actually adding the new data to the final database (bulk insert local, correct validation failures).
    /// </summary>
    public partial class _Default : Page
    {
        
        private ImportDirector eeemportDirector;
        ImportConfigurtator ic;

        protected void Page_Load( object sender, EventArgs e )
        {

        }

        protected void Button1_Click( object sender, EventArgs e )
        {
            var options = new DelimitedImportOptions() { Path = ConfigurationManager.AppSettings["ImportPath"], ColumnNamesInFirstRow = true, Delimiter = ',', TempFileCreationDir = ConfigurationManager.AppSettings["ImportTempDir"] };
            if ( CheckImport(options) )
            {
                try
                {
                    Label1.Text = "bout to make IC";
                    ic = new ImportConfigurtator( eeemportDirector );    // creates mappings assuming that there are column names that match target columwns.
                    Label1.Text = "Building validator list";
                    var validatorList = BuildStudentImportValidatorList();
                    var validationFailures = new List<IImportValidationFailure>();
                    Label1.Text = "Running validation";
                    ValidationExecution.Run( validationFailures, validatorList, ( ex ) => Label1.Text = ex.Message );
                    ic.ValidationFailures = validationFailures;

                    var vc = new ValidationFailureCorrector() { Config = ic };
                    Label1.Text = "About to correct failures";
                    var ticky = Environment.TickCount;
                    vc.Correct( new LoggerForTest(), false );
                    Label1.Text = string.Format( "Time to correct: {0}", Environment.TickCount - ticky );
                }
                catch ( Exception ex )
                {
                    Label1.Text = ex.Message;
                }
            }
            else
            {
                //Label1.Text = "CheckInmport failed";
            }
        }

        private List<IImportValidator> BuildStudentImportValidatorList()
        {
            return ValidationExecution.RegularImportValidatorList( ic );
        }

        private bool CheckImport( ImportOptions options )
        {
            Label1.Text = "Bout ta import";
            var result = false;
            eeemportDirector = new ImportDirector();
            try
            {
                result = eeemportDirector.ExecuteImport( ConfigurationManager.ConnectionStrings["ImportConnectionString"].ConnectionString, options );
            }
            catch ( Exception ex )
            {
                Label1.Text = ex.Message;
            }
            if ( result )
            {
                return true;
            }
            else
            {
                Label1.Text = eeemportDirector.GeneratedMessages.First();
                return false;
            }
        }
        
        private void StraightDBInsert()
        {
            using ( var connection = new SqlConnection( ConfigurationManager.ConnectionStrings["ImportConnectionString"].ConnectionString ) )
            {
                using ( var cmd = new SqlCommand( "create TABLE [dbo].[bobo]([id] int primary key IDENTITY(1,1),[STUDENTID] varchar(4000) NULL,[L_NAME] varchar(4000) NULL,[F_NAME] varchar(4000) NULL,[M_NAME] varchar(4000) NULL,[SADDRESS1] varchar(4000) NULL,[SADDRESS2] varchar(4000) NULL,[SCITY] varchar(4000) NULL,[STATE] varchar(4000) NULL,[SZIP_CODE] varchar(4000) NULL,[SHOMEPHONE] varchar(4000) NULL,[BIRTH_DT] varchar(4000) NULL,[GENDER] varchar(4000) NULL,[DATE_END] varchar(4000) NULL,[FLAG_PRO] varchar(4000) NULL,[DATE_PRO] varchar(4000) NULL,[FLAG_INQ] varchar(4000) NULL,[DATE_INQ] varchar(4000) NULL,[FLAG_APP] varchar(4000) NULL,[DATE_APP] varchar(4000) NULL,[FLAG_ADM] varchar(4000) NULL,[DATE_ADM] varchar(4000) NULL,[FLAG_CON] varchar(4000) NULL,[DATE_CON] varchar(4000) NULL,[FLAG_ENR] varchar(4000) NULL,[FLAG_CANC] varchar(4000) NULL,[ADMIT_CD] varchar(4000) NULL,[ADMIT_TYPE] varchar(4000) NULL,[ISOURCE_CD] varchar(4000) NULL,[ENTRY_STAT] varchar(4000) NULL,[HS_GRAD_YR] varchar(4000) NULL,[ENTRY_TERM] varchar(4000) NULL,[MARKET_SEG] varchar(4000) NULL,[COUNSELOR] varchar(4000) NULL,[ACADEM_LEV] varchar(4000) NULL,[COMMUTER] varchar(4000) NULL,[CHURCH_CD] varchar(4000) NULL,[DENOM_CD] varchar(4000) NULL,[ETHNIC_CD] varchar(4000) NULL,[LOADING] varchar(4000) NULL,[M_STATUS] varchar(4000) NULL,[MILITARY] varchar(4000) NULL,[OREN_SESS] varchar(4000) NULL,[ST_INT_LVL] varchar(4000) NULL,[SITE_CD] varchar(4000) NULL,[SUBINST_CD] varchar(4000) NULL,[TRAD] varchar(4000) NULL,[MAJOR1] varchar(4000) NULL,[DEPT1] varchar(4000) NULL,[MAJOR2] varchar(4000) NULL,[DEPT2] varchar(4000) NULL,[INTEREST1] varchar(4000) NULL,[INTEREST2] varchar(4000) NULL,[HSCODE] varchar(4000) NULL,[HSGPA] varchar(4000) NULL,[ACTSOURCE] varchar(4000) NULL,[ACTENGLISH] varchar(4000) NULL,[ACTMATH] varchar(4000) NULL,[ACTREADING] varchar(4000) NULL,[ACTSCIENCE] varchar(4000) NULL,[ACT_COMP] varchar(4000) NULL,[ACT_CHOICE] varchar(4000) NULL,[SATSOURCE] varchar(4000) NULL,[SATVERBAL] varchar(4000) NULL,[SATMATH] varchar(4000) NULL,[SAT_COMP] varchar(4000) NULL,[P_GPA] varchar(4000) NULL,[CONT_SI] varchar(4000) NULL,[CONT_TI] varchar(4000) NULL,[OP_HOUSE] varchar(4000) NULL,[CA_VISIT] varchar(4000) NULL,[USERDEF01] varchar(4000) NULL,[USERDEF02] varchar(4000) NULL,[USERDEF03] varchar(4000) NULL,[USERDEF04] varchar(4000) NULL,[USERDEF05] varchar(4000) NULL,[USERDEF06] varchar(4000) NULL,[USERDEF07] varchar(4000) NULL,[USERDEF08] varchar(4000) NULL,[USERDEF09] varchar(4000) NULL,[USERDEF10] varchar(4000) NULL,[USERDEF11] varchar(4000) NULL,[USERDEF12] varchar(4000) NULL,[USERDEF13] varchar(4000) NULL,[USERDEF14] varchar(4000) NULL,[USERDEF15] varchar(4000) NULL,[USERDEF16] varchar(4000) NULL,[USERDEF17] varchar(4000) NULL,[USERDEF18] varchar(4000) NULL,[USERDEF19] varchar(4000) NULL,[USERDEF20] varchar(4000) NULL,[USERDEF21] varchar(4000) NULL,[USERDEF22] varchar(4000) NULL,[USERDEF23] varchar(4000) NULL,[USERDEF24] varchar(4000) NULL,[USERDEF25] varchar(4000) NULL,[USERDEF26] varchar(4000) NULL,[USERDEF27] varchar(4000) NULL,[USERDEF28] varchar(4000) NULL,[USERDEF29] varchar(4000) NULL,[USERDEF30] varchar(4000) NULL,[USERDEF31] varchar(4000) NULL,[USERDEF32] varchar(4000) NULL,[USERDEF33] varchar(4000) NULL,[USERDEF34] varchar(4000) NULL,[USERDEF35] varchar(4000) NULL,[USERDEF36] varchar(4000) NULL,[USERDEF37] varchar(4000) NULL,[USERDEF38] varchar(4000) NULL,[USERDEF39] varchar(4000) NULL,[EMAIL] varchar(4000) NULL,[FLAG_PUSH] varchar(4000) NULL,[DATE_CANC] varchar(4000) NULL,[APP_CODE] varchar(4000) NULL,[APP_TYPE] varchar(4000) NULL,[MOBILE_PH] varchar(4000) NULL,[SEC_EMAIL] varchar(4000) NULL,[PAR_EMAIL] varchar(4000) NULL,[FLAG_ETHN] varchar(4000) NULL,[RACE1] varchar(4000) NULL,[RACE2] varchar(4000) NULL,[RACE3] varchar(4000) NULL,[RACE4] varchar(4000) NULL,[RACE5] varchar(4000) NULL,[RACE6] varchar(4000) NULL,[RACE7] varchar(4000) NULL,[RACE8] varchar(4000) NULL,[RACE9] varchar(4000) NULL,[RACE10] varchar(4000) NULL,[VENDOR] varchar(4000) NULL,[NUSERDEF1] varchar(4000) NULL,[NUSERDEF2] varchar(4000) NULL,[NUSERDEF3] varchar(4000) NULL,[NUSERDEF4] varchar(4000) NULL,[NUSERDEF5] varchar(4000) NULL,[NUSERDEF6] varchar(4000) NULL,[NUSERDEF7] varchar(4000) NULL,[NUSERDEF8] varchar(4000) NULL,[NUSERDEF9] varchar(4000) NULL,[NUSERDEF10] varchar(4000) NULL,[ACTWRITTEN] varchar(4000) NULL,[SATWRITTEN] varchar(4000) NULL,[SCOUNTY] varchar(4000) NULL,[SS_NUMBER] varchar(4000) NULL)", connection ) )
                {
                    try
                    {
                        connection.Open();

                        Label1.Text = "bouts ta create bobo";
                        cmd.ExecuteNonQuery();
                        
                        Label1.Text = "bouts ta insert";
                        cmd.CommandText = @"insert bobo select * from openrowset ( bulk '\\co-dt010\Import_LocalDb\Inquiry18K.txt', formatfile = '\\co-dt010\Import_LocalDb\format.xml', FIRSTROW = 2) a";
                        cmd.ExecuteNonQuery();

                        Label1.Text = "bouts ta update";
                        var ticky = Environment.TickCount;
                        cmd.CommandText = @"update bobo set act_comp = 'the quick brown fox jumped over the lazvy red dog'";
                        cmd.ExecuteNonQuery();

                        Label1.Text = ( Environment.TickCount - ticky ).ToString();
                    }
                    catch ( Exception ex )
                    {
                        Label1.Text = ex.Message;
                    }
                }
            }
        }

    }
}