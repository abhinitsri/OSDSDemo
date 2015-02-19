using NoelLevitz.Import.Helpers;
using NoelLevitz.Import.Validation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebDoImport
{
    /// <summary>
    /// Specific to FAS because it references FAS columns.
    /// </summary>
    public class RoundFloatsConverter : RoundIntsConverter
    {

        public static List<string> FLOATS_TO_ROUND_ACADEMIC = new List<string>( 8 ) { "C_Gpa", "C_Gpa_Recalc", "HsGpa", "HsGpa_Recalc", "Inst_Rat", "P_Gpa", "OtherTestScore1", "OtherTestScore2" };

        public RoundFloatsConverter()
        {
            _decimalPrecision = '3';
        }

        protected override void ProcessValidation()
        {
            using ( var connection = new SqlConnection( ConfigurationManager.ConnectionStrings["ImportConnectionString"].ConnectionString ) )
            {
                connection.Open();
                Mappings.ForEach( m =>
                {
                    if ( ShouldUpdate( m ) )
                    {
                        DoUpdate( m, connection );
                    }
                } );
            }
        }

        protected virtual bool ShouldUpdate( SimpleImportMap m )
        {
            return FLOATS_TO_ROUND_ACADEMIC.Contains( m.DestColumnName );
        }

    }
}