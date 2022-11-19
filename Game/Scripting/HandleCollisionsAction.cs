using System;
using System.Collections.Generic;
using System.Data;
using Unit05.Game.Casting;
using Unit05.Game.Services;


namespace Unit05.Game.Scripting
{
    /// <summary>
    /// <para>An update action that handles interactions between the actors.</para>
    /// <para>
    /// The responsibility of HandleCollisionsAction is to handle the situation when the snake 
    /// collides with the food, or the snake collides with its segments, or the game is over.
    /// </para>
    /// </summary>
    public class HandleCollisionsAction : Action
    {
        private bool isGameOver = false;
        private bool red_lost = false;
        private bool blue_lost = false;

        /// <summary>
        /// Constructs a new instance of HandleCollisionsAction.
        /// </summary>
        public HandleCollisionsAction()
        {
        }

        /// <inheritdoc/>
        public void Execute(Cast cast, Script script)
        {
            if (isGameOver == false)
            {
                HandleFoodCollisions(cast);
                HandleSegmentCollisions(cast);
                HandleGameOver(cast);
                Player snake = (Player)cast.GetFirstActor("BluePlayer");
                List<Actor> body = snake.GetBody();
                snake.GrowTail(1);

                Player snake2 = (Player)cast.GetFirstActor("RedPlayer");
                List<Actor> body2 = snake2.GetBody();
                snake2.GrowTail(1);
            }
        }

        /// <summary>
        /// Updates the score nd moves the food if the snake collides with it.
        /// </summary>
        /// <param name="cast">The cast of actors.</param>
        private void HandleFoodCollisions(Cast cast)
        {
            Player snake = (Player)cast.GetFirstActor("BluePlayer");
            Score score = (Score)cast.GetFirstActor("score");
            Food food = (Food)cast.GetFirstActor("food");
            
            if (snake.GetHead().GetPosition().Equals(food.GetPosition()))
            {
                int points = food.GetPoints();
                snake.GrowTail(points);
                score.AddPoints(points);
                food.Reset();
            }
        }

        /// <summary>
        /// Sets the game over flag if the snake collides with one of its segments.
        /// </summary>
        /// <param name="cast">The cast of actors.</param>
        private void HandleSegmentCollisions(Cast cast)
        {
            Player snake = (Player)cast.GetFirstActor("BluePlayer");
            Player snake2 = (Player)cast.GetFirstActor("RedPlayer");
            Actor head2 = snake2.GetHead();
            Actor head = snake.GetHead();

            List<Actor> body = snake.GetBody();

            List<Actor> body2 = snake2.GetBody();
            int number = body.Count;
            
            

            foreach (Actor segment in body)
            {
                if (segment.GetPosition().Equals(head.GetPosition()))
                {
                    isGameOver = true;
                    blue_lost = true;
                }
                else if (segment.GetPosition().Equals(head2.GetPosition()))
                {
                    isGameOver = true;
                    red_lost = true;
                }
            }
            foreach (Actor segment in body2)
            {
                if (segment.GetPosition().Equals(head2.GetPosition()))
                {
                    isGameOver = true;
                    red_lost = true;
                }
                if (segment.GetPosition().Equals(head.GetPosition()))
                {
                    isGameOver = true;
                    blue_lost = true;
                }
            }
        }

        private void HandleGameOver(Cast cast)
        {
            if (isGameOver == true)
            {
                Player snake = (Player)cast.GetFirstActor("BluePlayer");
                List<Actor> segments = snake.GetSegments();
                Food food = (Food)cast.GetFirstActor("food");

                // create a "game over" message
                int x = Constants.MAX_X / 2;
                int y = Constants.MAX_Y / 2;
                Point position = new Point(x, y);
                string final_message = "Who Lost?";
                if ((blue_lost == true) & (red_lost == true))
                {
                    final_message = "Tie!";
                }
                else if (red_lost == true)
                {
                    final_message = "Blue Won! Red Lost";
                }
                else if (blue_lost == true)
                {
                    final_message = "Red Won! Blue Lost";
                }

                

                Actor message = new Actor();
                message.SetText(final_message);
                message.SetPosition(position);
                cast.AddActor("messages", message);

                // make everything white
                foreach (Actor segment in segments)
                {
                    segment.SetColor(Constants.WHITE);
                }
                food.SetColor(Constants.WHITE);
            }
        }

    }
}