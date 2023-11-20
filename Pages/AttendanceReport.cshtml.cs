using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClassTrack.Pages
{
    public class AttendanceReport : PageModel
    {
        [BindProperty(SupportsGet = true)] public List<string> studentName { get; set; }

        [BindProperty(SupportsGet = true)] public List<int> studentID { get; set; }

        [BindProperty(SupportsGet = true)] public List<DateTime> timeOfSubmission { get; set; }

        public int studentCount { get; set; }

        public void OnGet()
        {
            const string connectionString =
                "Server=tcp:classtrack.database.windows.net,1433;Initial Catalog=LabDB;Persist Security Info=False;User ID=safey;Password=Saf4002ey_;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                var query = "SELECT count(*),* FROM USERINFO";
                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    studentCount = reader.GetInt32(0);
                    studentID.Add(reader.GetInt32(1));
                    studentName.Add(reader.GetString(2));
                    timeOfSubmission.Add(reader.GetDateTime(3));
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
    }
}