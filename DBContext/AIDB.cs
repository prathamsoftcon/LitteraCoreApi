using LitteraCore.Common;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LitteraCore.DBContext
{
    public class AIDB
    {
        private readonly IConfiguration _configuration;
        public AIDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool Save_AI_RESPONSE(AITool AI)
        {

            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_insert_tbl_tp_ai_data_collection", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@tttadc_session_id", AI.tttadc_session_id);
            cmd.Parameters.AddWithValue("@tttadc_conversation_id", AI.tttadc_conversation_id);
            cmd.Parameters.AddWithValue("@tttadc_text", AI.tttadc_text);
            cmd.Parameters.AddWithValue("@tttadc_reply", AI.tttadc_reply);
            if(AI.tttadc_islike != null)
            {
                cmd.Parameters.AddWithValue("@tttadc_islike", AI.tttadc_islike);
            }
            else
            {
                cmd.Parameters.AddWithValue("@tttadc_islike", 0);
            }
            if(AI.tttadc_remark != null)
            {
                cmd.Parameters.AddWithValue("@tttadc_remark", AI.tttadc_remark);
            }
            else
            {
                cmd.Parameters.AddWithValue("@tttadc_remark", DBNull.Value);
            }
            

            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        public bool Update_Like_Dislike(AITool AI)
        {

            List<User> user = new List<User>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_update_like_tbl_tp_ai_data_collection", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;
            cmd.Parameters.AddWithValue("@tttadc_session_id", AI.tttadc_session_id);
            cmd.Parameters.AddWithValue("@tttadc_conversation_id", AI.tttadc_conversation_id);

            if (AI.tttadc_islike != null)
            {
                cmd.Parameters.AddWithValue("@tttadc_islike", AI.tttadc_islike);
            }
            else
            {
                cmd.Parameters.AddWithValue("@tttadc_islike", 0);
            }
            
            if (AI.tttadc_remark != null)
            {
                cmd.Parameters.AddWithValue("@tttadc_remark", AI.tttadc_remark);
            }
            else
            {
                cmd.Parameters.AddWithValue("@tttadc_remark", DBNull.Value);
            }
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        public PagedList<AITool> Get_AI_Conversation(PaginationParam param)
        {

            List<AITool> f = new List<AITool>();
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("LitteraDatabase");
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlCommand cmd = new SqlCommand("trainingplan.proc_get_tbl_tp_ai_data_collection", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandTimeout = 5000;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                f.Add(
                    new AITool
                    {
                        tttadc_session_id = Convert.ToString(dr["tttadc_session_id"]),
                        tttadc_conversation_id = Convert.ToString(dr["tttadc_conversation_id"]),
                        tttadc_text= Convert.ToString(dr["tttadc_text"]),
                        tttadc_reply = Convert.ToString(dr["tttadc_reply"]),
                        tttadc_islike= Convert.ToInt16(dr["tttadc_islike"]),
                        tttadc_remark= Convert.ToString(dr["tttadc_remark"]),

                    });
            }

            if (param == null)
            {
                return PagedList<AITool>.ToPagedList(f.ToList(),
                   param.PageNumber,
                   f.Count());
            }
            else
            {
                if (param.PageSize > 0)
                {
                    return PagedList<AITool>.ToPagedList(f.ToList(),
                param.PageNumber,
                param.PageSize);
                }
                else
                {
                    return PagedList<AITool>.ToPagedList(f.ToList(),
                param.PageNumber,
               f.Count());
                }

            }


        }
    }
}
