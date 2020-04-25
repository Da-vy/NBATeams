using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NBATeams
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Maak een C#-WPF-applicatie waarin u de teams van uw favoriete sport kunt onderhouden. 
    /// Dat wil zeggen toevoegen, bekijken, aanpassen en verwijderen. 
    /// In een team zitten spelers en een team heeft een coach. 
    /// Maak in uw applicatie gebruik van objectoriëntatie en een database.
    /// </summary>
    public partial class MainWindow : Window
    {

        private string connectionString = @"data source = (LocalDB)\MSSQLLocalDB; attachdbfilename =|DataDirectory|\DATABASE\NBATEAMS.MDF; integrated security = True; connect timeout = 30; MultipleActiveResultSets = True; App = EntityFramework";

        NBATeamsEntities2 dataEntities = new NBATeamsEntities2();

        public string table = "";
        public string row = "";
        public string value = "";
               
        public MainWindow()
        {
            InitializeComponent();
            ComboBox1.SelectedIndex = 0;
            TextBox1.Text = "";
            TextBox2.Visibility = Visibility.Hidden;
            TextBlock2.Text = "Naam Team:";
            TextBlock3.Visibility = Visibility.Hidden;
            TextBlock4.Visibility = Visibility.Hidden;
            TextBlock5.Visibility = Visibility.Hidden;
            Button4.Visibility = Visibility.Hidden;
            Button5.Visibility = Visibility.Hidden;
            ComboBox2.Visibility = Visibility.Hidden;
            ComboBox3.Visibility = Visibility.Hidden;
            Select_Teams();

        }

        
        public void Select_Teams()
        {
            var query =
            from Teams in dataEntities.Teams
            select new { Teams.Team };

            DataGrid1.ItemsSource = query.ToList();
        }
              
        public void Select_Spelers()
        {
            var query =
            from Spelers in dataEntities.Spelers
            from Teams in dataEntities.Teams
            where Spelers.TeamID == Teams.TeamId
            select new { Spelers.Naam, Teams.Team };

            DataGrid1.ItemsSource = query.ToList();
        }

        public void Select_Coaches()
        {
            var query =
            from Coaches in dataEntities.Coaches
            from Teams in dataEntities.Teams
            where Coaches.TeamID == Teams.TeamId
            select new { Coaches.Naam, Teams.Team };

            DataGrid1.ItemsSource = query.ToList();
        }
        
        //Event die text bij TextBlock2 veranderd door gemaakte keuze bij ComboBox1
        //DropDownClosed="ComboBox_DropDownClosed" moest hiervoor gevuld worden bij ComboBox1 in xaml
        //zie ook https://stackoverflow.com/a/26675511/12371154
        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            
            if (ComboBox1.Text != "")
            {
                TextBox1.Visibility = Visibility.Visible;
                TextBlock2.Visibility = Visibility.Visible;
                Button1.Visibility = Visibility.Visible;
                Button2.Visibility = Visibility.Visible;
                Button3.Visibility = Visibility.Visible;
                DataGrid1.Visibility = Visibility.Visible;

                if (ComboBox1.Text == "Team")
                {
                    TextBlock2.Text = "Naam Team:";
                    TextBlock3.Visibility = Visibility.Hidden;
                    ComboBox2.Visibility = Visibility.Hidden;
                    Select_Teams();
                }
                else if (ComboBox1.Text == "Speler")
                {
                    TextBlock2.Text = "Naam Speler:";
                    TextBlock3.Visibility = Visibility.Visible;
                    ComboBox2.Visibility = Visibility.Visible;
                    Select_Spelers();
                }
                else if (ComboBox1.Text == "Coach")
                {
                    TextBlock2.Text = "Naam Coach:";
                    TextBlock3.Visibility = Visibility.Visible;
                    ComboBox2.Visibility = Visibility.Visible;
                    Select_Coaches();
                }
            }

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //update velden verbergen bij verandering in ComboBox1
            TextBox2.Visibility = Visibility.Hidden;
            TextBlock4.Visibility = Visibility.Hidden;
            ComboBox3.Visibility = Visibility.Hidden;
            TextBlock5.Visibility = Visibility.Hidden;
            Button4.Visibility = Visibility.Hidden;
            Button5.Visibility = Visibility.Hidden;
            

            //TextBox1 leegmaken bij verandering in ComboBox1
            TextBox1.Text = "";

            //ComboBox2 vullen met de Teams uit tabel Teams
            //Refresh bij elke verandering in ComboBox1
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Teams";
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    //Alles uit kolom "Team" toevoegen aan ComboBox2
                    ComboBox2.Items.Add(dataReader["Team"]);
                    ComboBox3.Items.Add(dataReader["Team"]);
                }

            }
            
        }    

        private void ComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //methode om waarde aan database toe te voegen
        //string table: aan welke tabel moet de waarde toegevoegd worden
        //string row: aan welke rij moet de waarde toegevoegd worden
        //string value: de waarde die aan de rij wordt toegevoegd. 
        public void AddToDatabase(string table, string row, string value)
        {
            string query = "INSERT INTO " + table + " (" + row + ") VALUES (@value)";
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //check of team al voorkomt in database
                connection.Open();
                SqlCommand check_team = new SqlCommand("SELECT COUNT(*) FROM [Teams] WHERE ([Team] = @value)", connection);
                check_team.Parameters.AddWithValue("@value", value);
                int TeamBestaat = (int)check_team.ExecuteScalar();

                if (TeamBestaat > 0)
                {
                    MessageBox.Show("Team bestaat al op de database!");
                }

                else
                {
                                    
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@value", value);
                
                    try
                    {                        
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 1)
                        {
                            MessageBox.Show(value + " aan tabel " + table + " toegevoegd!");
                        }
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }

            }
        }

        //Methode om team toe te voegen aan tabel Coaches of tabel Spelers
        //string table: aan welke tabel moet de waarde toegevoegd worden
        //string row: aan welke rij moet de waarde toegevoegd worden
        //string value: de waarde die aan de rij wordt toegevoegd. 
        private void AddTeam(string table, string row, string value)
        {
            //TeamID die hoort bij comboBox2.Text ophalen uit database.  
            string select = @"SELECT TeamId FROM Teams WHERE Team = @team";
            //value en TeamID toevoegen aan gekozen tabel en rij. 
            string insert = @"INSERT INTO " + table + "(" + row + " ,TeamID) VALUES (@value, @teamId)";
                        
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand selectCommand = new SqlCommand(select, connection);
                selectCommand.Parameters.AddWithValue("@team", ComboBox2.Text);
                SqlDataReader dataReader = selectCommand.ExecuteReader();

                string teamId = "";

                while (dataReader.Read())
                {
                    teamId = dataReader["TeamId"].ToString();
                }
                                

                SqlCommand insertCommand = new SqlCommand(insert, connection);
                insertCommand.Parameters.AddWithValue("@value", value);
                insertCommand.Parameters.AddWithValue("@teamId", teamId);

                try
                {
                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        MessageBox.Show(value + " aan tabel " + table + " toegevoegd!");
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //verwijderen van database
        private void Remove_From_Database(string table, string row, string value)
        {
            string query = "DELETE FROM " + table + " WHERE ([" + row + "] = @value)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //check of waarde voorkomt in database
                connection.Open();
                SqlCommand check_exists = new SqlCommand("SELECT COUNT(*) FROM [" + table + "] WHERE ([" + row + "] = @value)", connection);
                check_exists.Parameters.AddWithValue("@value", value);
                int Bestaat = (int)check_exists.ExecuteScalar();

                if (Bestaat > 0)
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@value", value);

                    //TeamID ophalen 
                    string select = @"SELECT TeamId FROM Teams WHERE Team = @team";

                    SqlCommand selectCommand = new SqlCommand(select, connection);
                    selectCommand.Parameters.AddWithValue("@team", TextBox1.Text);
                    SqlDataReader dataReader = selectCommand.ExecuteReader();

                    string teamId = "";

                    while (dataReader.Read())
                    {
                        teamId = dataReader["TeamId"].ToString();
                    }

                    try
                    {
                        //Team weghalen bij spelers en coaches en vervangen met TeamID 0: Geen Team
                        SqlCommand Remove_From_Coaches = new SqlCommand("UPDATE Coaches SET TeamID = 0 WHERE TeamID = " + teamId + "", connection);
                        Remove_From_Coaches.ExecuteNonQuery();

                        SqlCommand Remove_From_Spelers = new SqlCommand("UPDATE Spelers SET TeamID = 0 WHERE TeamID = " + teamId + "", connection);
                        Remove_From_Spelers.ExecuteNonQuery();

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 1)
                        {
                            MessageBox.Show(value + " verwijderd uit " + table + "!");
                        }
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                else
                {
                    MessageBox.Show("Bestaat niet op de database");
                }


            }
        }

        //verwijderen van database
        private void Remove_From_Database_Teams_Coaches(string table, string row, string value)
        {
            string query = "DELETE FROM " + table + " WHERE ([" + row + "] = @value)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //check of waarde voorkomt in database
                connection.Open();
                SqlCommand check_exists = new SqlCommand("SELECT COUNT(*) FROM [" + table + "] WHERE ([" + row + "] = @value)", connection);
                check_exists.Parameters.AddWithValue("@value", value);
                int Bestaat = (int)check_exists.ExecuteScalar();

                if (Bestaat > 0)
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@value", value);
                                        
                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 1)
                        {
                            MessageBox.Show(value + " verwijderd uit " + table + "!");
                        }
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                else
                {
                    MessageBox.Show("Bestaat niet op de database");
                }


            }
        }

        //Waarde wijzigen
        private void Update(string table, string row, string value, string newvalue)
        {
                    
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "";

                if (ComboBox3.Text != "")
                {
                    //TeamID ophalen 
                    string select = @"SELECT TeamId FROM Teams WHERE Team = @team";

                    SqlCommand selectCommand = new SqlCommand(select, connection);
                    selectCommand.Parameters.AddWithValue("@team", ComboBox3.Text);
                    SqlDataReader dataReader = selectCommand.ExecuteReader();

                    string teamId = "";

                    while (dataReader.Read())
                    {
                        teamId = dataReader["TeamId"].ToString();
                    }

                    query = "UPDATE " + table + " SET " + row + " = @newvalue, TeamID = " + teamId + " WHERE ([" + row + "] = @value)";
                }
                else
                {
                    query = "UPDATE " + table + " SET " + row + " = @newvalue WHERE ([" + row + "] = @value)";
                }

                //check of waarde voorkomt in database
                SqlCommand check_exists = new SqlCommand("SELECT COUNT(*) FROM [" + table + "] WHERE ([" + row + "] = @value)", connection);
                check_exists.Parameters.AddWithValue("@value", value);
                int Bestaat = (int)check_exists.ExecuteScalar();

                if (Bestaat > 0)
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@newvalue", newvalue);
                    command.Parameters.AddWithValue("@value", value);
                    

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 1)
                        {
                            MessageBox.Show(value + " is gewijzigd naar " + newvalue + "!");

                            TextBox2.Visibility = Visibility.Hidden;
                            TextBlock4.Visibility = Visibility.Hidden;
                            ComboBox3.Visibility = Visibility.Hidden;
                            TextBlock5.Visibility = Visibility.Hidden;
                            Button4.Visibility = Visibility.Hidden;
                            Button5.Visibility = Visibility.Hidden;

                            Button1.Visibility = Visibility.Visible;
                            Button2.Visibility = Visibility.Visible;
                            Button3.Visibility = Visibility.Visible;

                            TextBox1.Text = "";
                            ComboBox2.Text = "";
                        }
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                else
                {
                    MessageBox.Show("Er is niets gewijzigd.");
                }


            }
        }

        //insert button
        //Bij klikken op 'toevoegen' de ingevulde waarde toevoegen aan juiste tabel in database.
        //Juiste tabel wordt bepaald aan de hand van ComboBox1 Selectie.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (TextBox1.Text != "")
            {
                if (ComboBox1.Text == "Team")
                {
                    //zie methode AddToDatabase voor werking
                    AddToDatabase("Teams", "Team", TextBox1.Text);
                    DataGrid1.ItemsSource = null;
                    Select_Teams();
                }
                else if (ComboBox1.Text == "Speler")
                {
                    //zie methode AddTeam voor werking
                    AddTeam("Spelers", "Naam", TextBox1.Text);
                    DataGrid1.ItemsSource = null;
                    Select_Spelers();
                }
                else if (ComboBox1.Text == "Coach")
                {
                    //zie methode AddTeam voor werking
                    AddTeam("Coaches", "Naam", TextBox1.Text);
                    DataGrid1.ItemsSource = null;
                    Select_Coaches();
                }
                else
                {
                    MessageBox.Show("Maak een keuze a.u.b.");
                }
            }
                       
            else
            {
                MessageBox.Show("Veld niet leeglaten a.u.b.");
            }


        }


        //update button
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
                       
            TextBox2.Visibility = Visibility.Visible;
            TextBlock4.Visibility = Visibility.Visible;
            Button4.Visibility = Visibility.Visible;
            Button5.Visibility = Visibility.Visible;
            

            if (ComboBox1.Text != "Team")
            {
                ComboBox3.Visibility = Visibility.Visible;
                TextBlock5.Visibility = Visibility.Visible;
            }

            Button1.Visibility = Visibility.Hidden;
            Button2.Visibility = Visibility.Hidden;
            Button3.Visibility = Visibility.Hidden;


        }



        //delete button
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox1.Text != "")
            {
                if (ComboBox1.Text == "Team")
                {
                    //zie methode Remove_From_Database voor werking
                    Remove_From_Database("Teams", "Team", TextBox1.Text);
                    DataGrid1.ItemsSource = null;
                    Select_Teams();
                }
                else if (ComboBox1.Text == "Speler")
                {
                    //zie methode Remove_From_Database voor werking
                    Remove_From_Database_Teams_Coaches("Spelers", "Naam", TextBox1.Text);
                    DataGrid1.ItemsSource = null;
                    Select_Spelers();
                }
                else if (ComboBox1.Text == "Coach")
                {
                    //zie methode Remove_From_Database voor werking
                    Remove_From_Database_Teams_Coaches("Coaches", "Naam", TextBox1.Text);
                    DataGrid1.ItemsSource = null;
                    Select_Coaches();
                }
                else
                {
                    MessageBox.Show("Maak een keuze a.u.b.");
                }
            }
                     
            
        }

        //Confirm button
        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            //Inhoud TextBox1 is bestaande waarde.
            //Inhoud TextBox2 is vervanger van bestaande waarde TextBox1.
            //Eventueel met selectie ComboBox3 het team voor bestaande waarde updaten. 

            if (TextBox1.Text != "")
            {
                if (ComboBox1.Text == "Team")
                {
                    //zie methode update voor werking
                    Update("Teams", "Team", TextBox1.Text, TextBox2.Text);
                    DataGrid1.ItemsSource = null;
                    Select_Teams();
                }
                else if (ComboBox1.Text == "Speler")
                {
                    //zie methode update voor werking
                    Update("Spelers", "Naam", TextBox1.Text, TextBox2.Text);
                    DataGrid1.ItemsSource = null;
                    Select_Spelers();
                }
                else if (ComboBox1.Text == "Coach")
                {
                    //zie methode update voor werking
                    Update("Coaches", "Naam", TextBox1.Text, TextBox2.Text);
                    DataGrid1.ItemsSource = null;
                    Select_Coaches();
                }
                else
                {
                    MessageBox.Show("Maak een keuze a.u.b.");
                }
            }

            else
            {
                MessageBox.Show("Veld niet leeglaten a.u.b.");
            }
                       

        }

        //return button
        private void Button5_Click(object sender, RoutedEventArgs e)
        {

            TextBox2.Visibility = Visibility.Hidden;
            TextBlock4.Visibility = Visibility.Hidden;
            ComboBox3.Visibility = Visibility.Hidden;
            TextBlock5.Visibility = Visibility.Hidden;
            Button4.Visibility = Visibility.Hidden;
            Button5.Visibility = Visibility.Hidden;

            Button1.Visibility = Visibility.Visible;
            Button2.Visibility = Visibility.Visible;
            Button3.Visibility = Visibility.Visible;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                     
                        
        }


    }

    
}
