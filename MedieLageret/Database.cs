using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace MedieLageret
{
    public class Database
    {
        private string _connectionString = "ConnectionString"; //Standard connectionsstring fra webconfig
        private SqlConnection _conn = new SqlConnection(); //Ny tom dummy forbindelse
        private SqlDataAdapter _sql = new SqlDataAdapter(); //Ny tom dummy sql adapter

        #region connection
        /// <summary>
        /// En databaseklasse til at hente, indsætte, opdaterer og slette indhold, hvor connectionstring hedder "ConnectionString".
        /// </summary>
        public Database()
        {
            this.Connect();
        }
        /// <summary>
        /// En databaseklasse til at hente, indsætte, opdaterer og slette indhold, med egen forbindelse.
        /// </summary>
        /// <param name="connection"></param>
        public Database(string connection)
        {
            this._connectionString = connection;
            this.Connect();
        }
        /// <summary>
        /// Opretter forbindelse med den givne connectionsstring
        /// </summary>
        private void Connect()
        {
            this._conn.ConnectionString = ConfigurationManager.ConnectionStrings[this._connectionString].ConnectionString;
        }

        /// <summary>
        /// Opret ny forbindelse med den givne connectionstring, eller hent nuværende connectionstring.
        /// </summary>
        public string ConnectionString
        {
            get { return this._connectionString; }
            set { 
                this._connectionString = value;
                this.Connect();
            }
        }
        #endregion

        /// <summary>
        /// Tilføj bruger parametre til SqlCommand objektet.
        /// </summary>
        /// <param name="cmd">SqlCommand der skal tilføjes parametre til.</param>
        /// <param name="parameters">Liste med navne og værdier for de parametre der skal tilføjes.</param>
        /// <returns>SqlCommand objekt med tilhørende parametre</returns>
        private static SqlCommand SetParameters(SqlCommand cmd, Dictionary<string, object> parameters)
        {
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
            return cmd;
        }

        /// <summary>
        /// Bruges til at hente indhold fra databasen, med egen sql og valgri parametre
        /// </summary>
        /// <param name="sqlCode">SQL koden der skal udføres, men parametrenavne</param>
        /// <param name="parameters">En liste med parametre værdier, som matcher parametre navnene</param>
        /// <returns>En DataTable med indholdet fra den udførte Sql kode</returns>
        public DataTable Select(string sqlCode, Dictionary<string, object> parameters = null)
        {
            SqlCommand select = new SqlCommand(sqlCode, this._conn);

            //Tilføj dynamiske parametre til command objectet
            if (parameters != null)
            {
                select = SetParameters(select, parameters);
            }
            _sql.SelectCommand = select;
            
            //Udfør sql koden med valgte parametre
            DataTable dbContent = new DataTable();
            _sql.Fill(dbContent);
            return dbContent;
        }

        /// <summary>
        /// Bruges til at hente en enkelt række fra databasen.
        /// </summary>
        /// <param name="table">Tabel navnet der skal hentes fra</param>
        /// <param name="field">Feltet i databasen der skal spørges på</param>
        /// <param name="value">Værdier af det felt der spørges på.</param>
        /// <returns>En DataRow med en enkelt række fra, den udførte Sql kode</returns>
        public DataRow GetOne(string table, string field, object value)
        {
            DataTable content = this.GetMany(table,field,value);
            return content.Rows[0];
        }


        public DataTable GetMany(string table, string field, object value)
        {
            Dictionary<string, object> myParams = new Dictionary<string, object>();
            myParams.Add("value", value);

            string formatedSql = string.Format("SELECT * FROM {0} WHERE {1}=@value", table, field);
            return this.Select(formatedSql, myParams);

        }

        /// <summary>
        /// Bruges til at hente alt fra en enkelt tabel i databasen
        /// </summary>
        /// <param name="table">Navnet på den tabel der skal hentes fra</param>
        /// <returns>En DataTable med alt indhold fra den enkelte tabel</returns>
        public DataTable GetAll(string table)
        { 
            return this.Select("SELECT * FROM " + table);
        }

        /// <summary>
        /// Bruges til at indsætte en enkelt række i databasen
        /// </summary>
        /// <param name="table">Navnet på den tabel der skal indsættes i</param>
        /// <param name="parameters">De værdier der skal indsættes i databasen. Navnene skal matche de navne der er i databasen.</param>
        /// <returns>Id'et fra den række der lige er sat ind i databasen </returns>
        public int Insert(string table, Dictionary<string, object> parameters)
        {
            string parameterNames = "@" + string.Join(",@", parameters.Keys);
            string formatedSql = string.Format("INSERT INTO {0} ({1}) VALUES ({2}); SELECT SCOPE_IDENTITY()", table, string.Join(",", parameters.Keys), parameterNames);
           
            SqlCommand insert = new SqlCommand(formatedSql, this._conn);
            insert = SetParameters(insert, parameters);
            _sql.InsertCommand = insert;

            this._conn.Open();
            int id = int.Parse(_sql.InsertCommand.ExecuteScalar().ToString());
            this._conn.Close();

            return id;
        }

        public int Update(string table, string field, object value, Dictionary<string, object> parameters)
        {
            //Lav update sql sætningen, så alle værdier og parametre kommer med.
            string[] updateArray = new string[parameters.Count];
            int i = 0;
            foreach(KeyValuePair<string,object> param in parameters)
            {
                updateArray.SetValue(param.Key + "=@" + param.Key, i);
                i++;
            }

            string formatedSql = string.Format("UPDATE {0} SET {1} WHERE {2}=@value",table, string.Join(",",updateArray),field);

            //Indsæt værdier på alle parametre
            SqlCommand update = new SqlCommand(formatedSql, this._conn);
            parameters.Add("value", value);
            update = SetParameters(update, parameters);
            _sql.UpdateCommand = update;

            //Udført
            this._conn.Open();
            int id = int.Parse(_sql.UpdateCommand.ExecuteNonQuery().ToString());
            this._conn.Close();

            return id;
        }

        /// <summary>
        /// Sletter en eller flere rækker i databasen, ud fra den givne sql kode.
        /// </summary>
        /// <param name="table">Navnet på den tabel der skal slettes fra</param>
        /// <param name="field">Feltet som der slettes ud fra</param>
        /// <param name="value">Værdien af det der skal slettes ud fra</param>
        /// <returns></returns>
        public int Delete(string table, string field, object value)
        {
            Dictionary<string, object> myParams = new Dictionary<string, object>();
            myParams.Add("value", value);

            string formatedSql = string.Format("DELETE FROM {0} WHERE {1}=@value", table, field);

            SqlCommand delete = new SqlCommand(formatedSql, this._conn);
            delete = SetParameters(delete, myParams);
            _sql.DeleteCommand = delete;

            this._conn.Open();
            int affectedRows = _sql.DeleteCommand.ExecuteNonQuery();
            this._conn.Close();

            return affectedRows;
        }

    }
}