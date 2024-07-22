using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AlarmWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        //Un cercle
        private Ellipse ellipse;
        //3 aiguilles
        private Line minutes;
        private Line hours;
        private Line seconds;


        public MainWindow()
        {
            InitializeComponent();
            initialisation();


            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Vérifier l'heure toutes les secondes
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {

            
   

            double longueurAiguilleSeconde = ellipse.Width / 2;
            seconds.X2 = ellipse.Width / 2 + Math.Cos(15 * Math.PI / 30 - DateTime.Now.Second * Math.PI / 30) * longueurAiguilleSeconde;
            seconds.Y2 = ellipse.Height / 2 + Math.Sin(-15 * Math.PI / 30 + DateTime.Now.Second * Math.PI / 30) * longueurAiguilleSeconde;

            double longueurAiguilleminutes = ellipse.Width / 3;
            minutes.X2 = ellipse.Width / 2 + Math.Cos(15 * Math.PI / 30 - DateTime.Now.Minute * Math.PI / 30) * longueurAiguilleminutes;
            minutes.Y2 = ellipse.Height / 2 + Math.Sin(-15 * Math.PI / 30 + DateTime.Now.Minute * Math.PI / 30) * longueurAiguilleminutes;

            double longueurAiguillehours= ellipse.Width / 4;
            hours.X2 = ellipse.Width / 2 + Math.Cos(15 * Math.PI / 6 - DateTime.Now.Hour * Math.PI / 6) * longueurAiguillehours;
            hours.Y2 = ellipse.Height / 2 + Math.Sin(-15 * Math.PI / 6 + DateTime.Now.Hour * Math.PI / 6) * longueurAiguillehours;


            DateTime heureActuelle = DateTime.Now;

            // Parcourir chaque élément de la ListBox
            foreach (var item in ListBoxAlarmes.Items)
            {
                // Convertir l'élément en chaîne de caractères
                string alarmeString = item.ToString();

                // Convertir la chaîne de caractères en objet DateTime
                DateTime heureAlarme = DateTime.Parse(alarmeString);

                // Vérifier si l'heure actuelle correspond à l'heure de l'alarme
                if (heureActuelle.Hour == heureAlarme.Hour && heureActuelle.Minute == heureAlarme.Minute)
                {
                    // Déclencher l'alarme
                    DeclencherAlarme();
                    break; // Sortir de la boucle dès qu'une alarme est déclenchée
                }
            }



        }


        private void initialisation()
        {
            timer = new DispatcherTimer();
            //Définit combien de secondes entre chaque déclenchement de l'événement Tick 
            timer.Interval = TimeSpan.FromSeconds(1);
            //Associe une procédure événementielle à l'événement Tick du Timer, il vous faut écrire cette procédure événementielle
            timer.Tick += timer_Tick;
            //Lance le Timer, obligatoire sinon rien ne se passe
            timer.Start();

            


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            hour.Text = "00";
            minute.Text = "00";
            ellipse = new Ellipse();
            CNVClock.Children.Add(ellipse);

            ellipse.Width = 300;
            ellipse.Height = 300;
            byte red = 143;
            byte green = 70;
            byte blue = 187;
            Color customColor = Color.FromArgb(255, red, green, blue);
            SolidColorBrush brush = new SolidColorBrush(customColor);
            ellipse.Stroke = brush;
            ellipse.StrokeThickness = 3;

           



            seconds = new Line();
            CNVClock.Children.Add(seconds);
            seconds.Width = 300;
            seconds.Height = 300;
            seconds.Stroke = Brushes.Black;
            seconds.StrokeThickness = 1;
            seconds.X1 = ellipse.Width / 2;
            seconds.Y1 = ellipse.Height / 2;




            // Création de l'aiguille des minutes
            minutes = new Line();
            CNVClock.Children.Add(minutes);
            minutes.Width = 300;
            minutes.Height = 300;
            minutes.Stroke = Brushes.Violet;
            minutes.StrokeThickness = 2;
            minutes.X1 = ellipse.Width / 2;
            minutes.Y1 = ellipse.Height / 2;





            // Création de l'aiguille des heures
            hours = new Line();
            CNVClock.Children.Add(hours);
            hours.Width = 300;
            hours.Height = 300;

            hours.Stroke = Brushes.Yellow;
            hours.StrokeThickness = 3;
            hours.X1 = ellipse.Width / 2;
            hours.Y1 = ellipse.Height / 2;




        }
        private void addalarme_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs des TextBox "hour" et "minute"
            string hourValue = hour.Text;
            string minuteValue = minute.Text;

            // Créer une chaîne de caractères représentant l'heure au format désiré
            string heureTexte = $"{hourValue:D2}:{minuteValue:D2}";

            // Ajouter l'heure à la ListBox
            ListBoxAlarmes.Items.Add(heureTexte);

            // Vider les TextBox
            hour.Text = "";
            minute.Text = "";
        }
        private void removealarme_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si un élément est sélectionné dans la ListBox
            if (ListBoxAlarmes.SelectedItem != null)
            {
                // Supprimer l'élément sélectionné de la ListBox
                ListBoxAlarmes.Items.Remove(ListBoxAlarmes.SelectedItem);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une alarme à supprimer.", "Aucune alarme sélectionnée", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /*le déclenchement visuel reste les 60 secondes déclenché meme si on clique sur l'alarme ca revient à l'etat normal et tout de suite revient eu déclenchemennt
      car on est encore dans l'intervalle de l'alarme (60s) a partir des 60 secondes quand on double  clique sur l'alarme selectionnée sur listbox ca revient a son etat actuelle*/
        private void DeclencherAlarme()
        { 
            ellipse.StrokeThickness = 10;

        }
        private void ListBoxAlarmes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Réinitialiser l'état de l'alarme
            ReinitialiserAlarme();
        }

        private void ReinitialiserAlarme()
        {
            // Réinitialiser les propriétés de l'ellipse à leur état normal
            ellipse.StrokeThickness = 3;
        }




    }
}
