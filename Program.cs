﻿using Unit05.Game.Casting;
using Unit05.Game.Directing;
using Unit05.Game.Scripting;
using Unit05.Game.Services;



namespace Unit05.Game
{
    /// <summary>
    /// The program's entry point.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Starts the program using the given arguments.
        /// </summary>
        /// <param name="args">The given arguments.</param>
        static void Main(string[] args)
        {
            // create the cast
            Cast cast = new Cast();
            cast.AddActor("food", new Food()); //possible boosts later
            cast.AddActor("BluePlayer", new Player(Constants.BLUE, 45, 300));
            cast.AddActor("RedPlayer", new Player(Constants.RED, 450, 300));

            cast.AddActor("score", new Score());// dont need score

            // create the services
            KeyboardService keyboardService = new KeyboardService();
            VideoService videoService = new VideoService(true);
           
            // create the script
            Script script = new Script();
            script.AddAction("input", new ControlActorsAction(keyboardService));
            script.AddAction("update", new MoveActorsAction());
            script.AddAction("update", new HandleCollisionsAction());
            script.AddAction("output", new DrawActorsAction(videoService));

            // start the game
            Director director = new Director(videoService);
            director.StartGame(cast, script);
        }
    }
}