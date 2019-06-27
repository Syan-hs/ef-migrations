using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMigrations.DataAccess.ClientMigrations
{
    public abstract class BaseMigration : DbMigration
    {

        private static string _databaseName;

        /// <summary>
        /// Include database names are the names of database you want to run the migration
        /// </summary>
        protected abstract ExecuteMigrationOption ExecuteMigrationOption { get; }

        private string DataBaseName
        {
            get
            {
                if (_databaseName == null)
                    _databaseName = GetDatabaseName().ToLower();
                return _databaseName;
            }
        }

        protected string GetDatabaseName()
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EfMigrationContext"].ConnectionString))
            {
                using (var adapter = new SqlDataAdapter("SELECT DB_NAME() AS CurrentDatabase ", con))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt.Rows[0][0].ToString();
                }
            }
        }

        public bool CanRunMigration()
        {
            if (this.ExecuteMigrationOption?.DataBases?.Any() == true)
            {
                if (this.ExecuteMigrationOption.IgnoreMigration)
                {
                    return !this.ExecuteMigrationOption.DataBases.Any(a => this.DataBaseName.Contains(a.ToLower()));
                }
                else
                {
                    return this.ExecuteMigrationOption.DataBases.Any(a => this.DataBaseName.Contains(a.ToLower()));
                }
            }
            return true;
        }

        public sealed override void Up()
        {
            if (CanRunMigration())
            {
                UpMigration();
            }
        }

        public sealed override void Down()
        {
            if (CanRunMigration())
            {
                DownBackMigration();
            }
        }

        public abstract void UpMigration();
        public abstract void DownBackMigration();
    }

    public class ExecuteMigrationOption
    {
        //Specify database names
        public IList<string> DataBases { get; set; }
        //Flag to determine whether to execute/not to execute migration on database specified in <DataBases> property
        public bool IgnoreMigration { get; set; }
    }
}
