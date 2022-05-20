using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class MatchingGame : Form
    {
        Label started = null;
        private int _time = 0;
        private int min = 0;
        private int hour = 0;
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
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if(started == null)
            {
                timer2.Start();
            }
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
                    return;
                }

                /* if the player gets this far, the player
                   clicked two different icons, so start the
                   timer (which will wait three quarters of 
                   second, and then hide the icons) */
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

            //if the loop didn't return, it didn't find
            //any unmatched icons
            //that means the user won. Show a message and close the form
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
            MessageBox.Show("You matched all the icons!\nTime: "+hour+" hour "+min+" min "+_time+" seconds", "Congratulations");
            Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            _time++;
        }
    }
}
