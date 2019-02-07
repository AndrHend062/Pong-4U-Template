/*
 * Description:     A basic PONG simulator
 * Author:           
 * Date:            
 */

#region libraries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

#endregion

namespace Pong
{
    public partial class Form1 : Form
    {
        #region global values

        //graphics objects for drawing
        SolidBrush drawBrush = new SolidBrush(Color.White);
        Font drawFont = new Font("Courier New", 10);

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);

        //determines whether a key is being pressed or not
        Boolean aKeyDown, zKeyDown, jKeyDown, mKeyDown;

        // check to see if a new game can be started
        Boolean newGameOk = true;

        //ball directions, speed, and rectangle
        Boolean ballMoveRight = true;
        Boolean ballMoveDown = true;
        int BALL_SPEED = 2;
        Rectangle ball;
        int faster = 0;
        //paddle speeds and rectangles
         int PADDLE_SPEED = 3;
        Rectangle p1, p2;

        //player and game scores
        int player1Score = 0;
        int player2Score = 0;
        int gameWinScore = 3;  // number of points needed to win game

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.A:
                    aKeyDown = true;
                    break;
                case Keys.Z:
                    zKeyDown = true;
                    break;
                case Keys.J:
                    jKeyDown = true;
                    break;
                case Keys.M:
                    mKeyDown = true;
                    break;
                case Keys.Y:
                case Keys.Space:
                    if (newGameOk)
                    {
                        SetParameters();
                    }
                    break;
                case Keys.N:
                    if (newGameOk)
                    {
                        Close();
                    }
                    break;
            }
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.A:
                    aKeyDown = false;
                    break;
                case Keys.Z:
                    zKeyDown = false;
                    break;
                case Keys.J:
                    jKeyDown = false;
                    break;
                case Keys.M:
                    mKeyDown = false;
                    break;
            }
        }

        /// <summary>
        /// sets the ball and paddle positions for game start
        /// </summary>
        private void SetParameters()
        {
            if (newGameOk)
            {
                player1Score = player2Score = 0;
                newGameOk = false;
                startLabel.Visible = false;
                gameUpdateLoop.Start();
            }
            //speed of ball and padles 
            BALL_SPEED = 2;
            PADDLE_SPEED = 3;
            faster = 0;
            //set starting position for paddles on new game and point scored 
            const int PADDLE_EDGE = 20;  // buffer distance between screen edge and paddle            

            p1.Width = p2.Width = 10;    //height for both paddles set the same
            p1.Height = p2.Height = 40;  //width for both paddles set the same

            //p1 starting position
            p1.X = PADDLE_EDGE;
            p1.Y = this.Height / 2 - p1.Height / 2;

            //p2 starting position
            p2.X = this.Width - PADDLE_EDGE - p2.Width;
            p2.Y = this.Height / 2 - p2.Height / 2;

            //  set Width and Height of ball
            ball.Width = ball.Height = 5;
            //  set starting X position for ball to middle of screen, (use this.Width and ball.Width)
            ball.X = this.Width / 2 - ball.Width / 2;
            //  set starting Y position for ball to middle of screen, (use this.Height and ball.Height)
            ball.Y = this.Height / 2 - ball.Height / 2;
        }

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>
        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {
            #region update ball position

            //  create code to move ball either left or right based on ballMoveRight and using BALL_SPEED
            if (ballMoveRight)
            {
                ball.X = ball.X + BALL_SPEED;

            }
            else if (!ballMoveRight)
            { ball.X = ball.X - BALL_SPEED; }
            if (ballMoveDown)
            {
                ball.Y = ball.Y + BALL_SPEED;
            }
            else if (!ballMoveDown)
            { ball.Y = ball.Y - BALL_SPEED; }
            //  create code move ball either down or up based on ballMoveDown and using BALL_SPEED

            #endregion

            #region update paddle positions

            if (aKeyDown == true && p1.Y > 0)
            {
                //  create code to move player 1 paddle up using p1.Y and PADDLE_SPEED
                p1.Y = p1.Y - PADDLE_SPEED;
            }

            //  create an if statement and code to move player 1 paddle down using p1.Y and PADDLE_SPEED
            if (zKeyDown == true && p1.Y < this.Height - p2.Height)
            {
                p1.Y = p1.Y + PADDLE_SPEED;
            }
            //  create an if statement and code to move player 2 paddle up using p2.Y and PADDLE_SPEED
            if (jKeyDown == true && p2.Y > 0)
            {

                p2.Y = p2.Y - PADDLE_SPEED;
            }
            //  create an if statement and code to move player 2 paddle down using p2.Y and PADDLE_SPEED
            if (mKeyDown == true && p2.Y < this.Height - p2.Height)
            {

                p2.Y = p2.Y + PADDLE_SPEED;
            }
            #endregion

            #region ball collision with top and bottom lines

            if (ball.Y < 0) // if ball hits top line
            {
                ballMoveDown = true;
                collisionSound.Play();
                //  use ballMoveDown boolean to change direction
                //  play a collision sound
            }

            //  In an else if statement use ball.Y, this.Height, and ball.Width to check for collision with bottom line
            // If true use ballMoveDown down boolean to change direction
            if (ball.Y > this.Height - 5) // if ball hits top line
            {
                ballMoveDown = false;
                //  use ballMoveDown boolean to change direction
                //  play a collision sound
                collisionSound.Play();
            }
            #endregion

            #region ball collision with paddles

            // create if statment that checks p1 collides with ball and if it does
          
            // --- play a "paddle hit" sound and
            // --- use ballMoveRight boolean to change direction

            // create if statment that checks p2 collides with ball and if it does
            if (ball.IntersectsWith(p2)|| ball.IntersectsWith(p1))
            {
                collisionSound.Play();
                ballMoveRight = !ballMoveRight;
                faster++;
                if(faster > 4)
                {
                    PADDLE_SPEED= PADDLE_SPEED*2;
                    BALL_SPEED=BALL_SPEED*2;
                    faster = 0;
                }
            }
            // --- play a "paddle hit" sound and
            // --- use ballMoveRight boolean to change direction


            /*  ENRICHMENT
             *  Instead of using two if statments as noted above see if you can create one
             *  if statement with multiple conditions to play a sound and change direction
             */

            #endregion

            #region ball collision with side walls (point scored)

            if (ball.X < 0)  // ball hits left wall logic
            {
                scoreSound.Play();
                // --- play score sound
                // --- update player 2 score
                player2Score++;
                //  use if statement to check to see if player 2 has won the game. If true run 

                // GameOver method. Else change direction of ball and call SetParameters method.
                if (player2Score == gameWinScore)
                {
                    
                    GameOver("Player 2 \n Has Defeated Player 1");

                }

                else
                {
                    ballMoveRight = !ballMoveRight;
                    SetParameters();
                }
                
            }

            //  same as above but this time check for collision with the right wall
            if (ball.X > this.Width)
            {
                player1Score++;
                scoreSound.Play();
                if (player1Score == gameWinScore)
                {
                  
                    GameOver("Player 1 \n Has Defeated Player 2");
                }
                else
                {
                    SetParameters();
                    ballMoveRight = !ballMoveRight;
                }
            }
           
            #endregion

            //refresh the screen, which causes the Form1_Paint method to run
            this.Refresh();
        }
        
        /// <summary>
        /// Displays a message for the winner when the game is over and allows the user to either select
        /// to play again or end the program
        /// </summary>
        /// <param name="winner">The player name to be shown as the winner</param>
        private void GameOver(string winner)
        {
            newGameOk = true;
            startLabel.Visible = true;
            startLabel.Text = winner;
            //  create game over logic
            gameUpdateLoop.Stop();
            this.Refresh();
            Thread.Sleep(2000);
            startLabel.Text = "Play Again?";
            // --- stop the gameUpdateLoop
            // --- show a message on the startLabel to indicate a winner, (need to Refresh).
            // --- pause for two seconds 
            // --- use the startLabel to ask the user if they want to play again

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //  draw paddles using FillRectangle
            e.Graphics.FillRectangle(drawBrush,p1);
            e.Graphics.FillRectangle(drawBrush, p2);
            //  draw ball using FillRectangle
            e.Graphics.FillRectangle(drawBrush, ball);
            //  draw scores to the screen using DrawString
            e.Graphics.DrawString(player1Score +"", drawFont,drawBrush, this.Width/5,10 );
            e.Graphics.DrawString(player2Score + "", drawFont, drawBrush, this.Width - this.Width / 5, 10);
        }

    }
}
