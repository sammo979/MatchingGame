using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace MatchingGame
{
    public partial class MatchingGame : Form
    {
        //add-ons, Sound
        private SoundPlayer goodMatch;
        private SoundPlayer badMatch;
        private SoundPlayer won;
        //add-ons,end

        //Add-ons, Timer
        Label started = null; // to know if a game has started
        private int _time = 0; // for the timer, each time it ticks we add one 
        private int min = 0; // for calcuating the time after the game ends
        private int hour = 0; //for calcuating the time after the game ends
        //Add-ons, end

        /* firstClicked points to the frist label control
         * that the player clicks, but it will be null
         * if the player hasn't clicked a label yet */
        Label firstClicked = null;

        //secondClicked points to the second label control that the player clicks
        Label secondClicked = null;

        //use this Random object to choose random icons for the squares
        Random random = new Random();

        //Each of these letters is an interesting icon
        //in the Webdings font,
        //and each icon appears twice in this list
        List<string> icons = new List<string>()
        {
            "!","!","N","N",",",",","k","k","b","b","v","v","w","w","z","z"
        };
        private void AssignIconsToSquares()
        {
            /* The TableLayoutPanel has 16 labels,
             * and the icon list has 16 icons,
             * so an icon is pulled at random from the list
             * and added to each label
             */
            foreach(Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label; //converts the control var to a label
                if(iconLabel != null) //makes sure the conversion worked
                {
                    int randomNumber = random.Next(icons.Count); //gets a random number in the range of icon list
                    iconLabel.Text = icons[randomNumber]; //set the text to the random icon from the icon list with the random num
                    iconLabel.ForeColor = iconLabel.BackColor; //this hides the icons from the player
                    icons.RemoveAt(randomNumber);//removes the icon from the list that we just placed on the board
                }
            }
        }
        public MatchingGame()
        {
            InitializeComponent();
            AssignIconsToSquares();
            goodMatch = new SoundPlayer("good.wav");
            badMatch = new SoundPlayer("bad.wav");
            won = new SoundPlayer("won.wav");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //add-ons, timer, if this is the first time the player had clicked on a square then start the timer
            if(started == null)
            {
                timer2.Start();
            }
            //add-ons end

            //the timer is only on after two no-matching icons
            // have been shown to the player,
            // so ignore any clicks if the timer is running
            if(timer1.Enabled == true)
            {
                return;
            }
            //every label's click event is handled by this event handler
            Label clickedLabel = sender as Label;
            if(clickedLabel != null)
            {
                /* if the clicked label is black, the player clicked
                 * an icon that's already been revealed --
                 * ignore the click
                 */
                if(clickedLabel.ForeColor == Color.Black)
                {
                    return;
                }

                /* if firstClicked is null, this is the first icon 
                 * in the pair that the player clicked,
                 * so set firstClicked to the label that the player
                 * clicked, change its color to black, and return*/
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                /* if the player gets this far, the timer isn't
                 running and firstClicked isn't null,
                 so this must be the second icon the player clicked
                 set its color to black */
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                //check to see if the player won
                CheckForWinner();

                /* If the player clicked two matching icons, keep them
                   black and reset firstClicked and secondClicked
                   so the player can click another icon */
                if(firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    goodMatch.Play();
                    return;
                }

                /* if the player gets this far, the player
                   clicked two different icons, so start the
                   timer (which will wait three quarters of 
                   second, and then hide the icons) */
                badMatch.Play();
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //stop the timer
            timer1.Stop();

            //hide both icons
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            //reset firstclicked and secondclicked so the next time a label is clicked, the program knows it's the first click
            firstClicked = null;
            secondClicked = null;
        }
        private void CheckForWinner()
        {
            //go through all of the labels in the TableLayoutPanel
            //checking each one to see if its icon is matched
            foreach(Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if(iconLabel != null)
                {
                    if(iconLabel.ForeColor == iconLabel.BackColor)
                    {
                        return;
                    }
                }
            }

            //Add-ons,timer, if the player has won then stop the time
            // and then change _time into the format hh:mm:ss
            timer2.Stop();
            while(_time >= 60)
            {
                min++;
                _time-= 60;
            }
            while(min >= 60)
            {
                hour++;
                min-= 60;
            }
            //Add-ons End

            /* if the loop didn't return, it didn't find
               any unmatched icons
               that means the user won. Show a message and close the form 
               I added time to the message to display the time it took to win the game */
            won.Play();
            MessageBox.Show("You matched all the icons!\nTime: "+hour+" hour "+min+" min "+_time+" seconds", "Congratulations");
            Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //add-ons, timer
            _time++;
        }
    }
}
