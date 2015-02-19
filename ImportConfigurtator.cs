using NoelLevitz.ForecastPlus.Data.Models;
using NoelLevitz.Import.Helpers;
using NoelLevitz.Import.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDoImport
{
    public class ImportConfigurtator: IImportConfigurator
    {

        private IImportDirector _id;
        private IEnumerable<IImportValidationFailure> _validationFailures;

        public ImportConfigurtator( IImportDirector id )
        {
            _id = id;
            SimpleImportMappings_yo();
        }

        public bool IsMandatory( string field )
        {
            return false;
        }

        public string LocalTablePrimaryKeyColumn
        {
            get { return LocalTableName.Replace( "#", string.Empty ); }
        }

        public string LocalTableName
        {
            get { return _id.GeneratedTableName; }
        }

        public List<NoelLevitz.Import.Helpers.SimpleImportMap> SimpleImportMappings
        {
            get { return SimpleImportMappings_yo(); }
        }

        private List<SimpleImportMap> SimpleImportMappings_yo()
        {
            var fields = LoadFields();

            if ( fields.Any() )
            {
                var query = from fm in _id.GeneratedColumns
                            from f in fields.Where( innerF => fm.ToUpper() == innerF.Name.ToUpper() || fm.ToUpper() == (string.IsNullOrEmpty(innerF.LegacyName) ? "" : innerF.LegacyName.ToUpper()) )
                            select new SimpleImportMap()
                            {
                                Source = fm,
                                DestColumnName = f.Name,
                                DestEntityName = f.TableName,
                                DestType = f.DataType,
                                DestWidth = f.Width,
                                RowBased = false
                            };
                return query.ToList();
            }
            else
            {
                throw new Exception( "no fields" );
            }
        }

        private static List<Field> LoadFields()
        {
            using ( var context = new ForecastContext(true) )
            {
                return context.Fields.Where( f => f.LayoutFieldCategories.Any( lfc => lfc.Layout.Name == "Inquiry" ) ).ToList();
            }
        }

        public string RowCountToBeImported
        {
            get { return _id.RowsImported.ToString(); }
        }

        public bool UpdateNullData
        {
            get { throw new NotImplementedException(); }
        }

        public string FilePath
        {
            get { throw new NotImplementedException(); }
        }

        public NoelLevitz.Import.Enumerations.ImportActionType SelectedImportAction
        {
            get { throw new NotImplementedException(); }
        }

        public string DatasetDescription
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime TransmitDate
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsCensus
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IImportValidationFailure> ValidationFailures
        {
            get { return _validationFailures; }
            set { _validationFailures = value; }
        }
    }
}