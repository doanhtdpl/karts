using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Karts.Code.SceneManager;
using Karts.Code.SceneManager.Components;

namespace Karts.Code
{
    class Lobby : GameState
    {
        private Screen menu;
        private List<TextComponent> players = new List<TextComponent>();

        public override void Enter()
        {
            menu = new Screen();
            Gui.GetInstance().AddComponent(menu);

            foreach (Player player in PlayerManager.GetInstance().GetPlayers())
            {
                TextComponent comp = new TextComponent(100, 150 + players.Count * 50, player.GetName());
                players.Add(comp);
                menu.AddComponent(comp);
            }

            base.Enter();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime GameTime)
        {
            if (ControllerManager.GetInstance().isPressed(PlayerManager.GetInstance().ActivePlayerIndex, "menu_cancel")){
                GoBack();
            }else if(ControllerManager.GetInstance().isPressed(PlayerManager.GetInstance().ActivePlayerIndex, "menu_ok")){
                Confirm();
            }else if (NetworkManager.GetInstance().IsClient()){
                if (NetworkManager.GetInstance().GetSession().SessionState == NetworkSessionState.Playing)
                {
                    GameStateManager.GetInstance().ChangeState(new GameplayState());
                }
            }else{
                CheckLocalJoins();

                CheckNetworkJoins();
            }
            base.Update(GameTime);
        }

        private void GoBack(){
            //Leave session if any
            NetworkManager.GetInstance().LeaveSession();
            PlayerManager.GetInstance().RemovePlayers();

            GameStateManager.GetInstance().ChangeState(new MainMenu());
        }

        private void Confirm(){
            if(NetworkManager.GetInstance().HasSession()){
                if(NetworkManager.GetInstance().GetSession().IsHost){
                    NetworkManager.GetInstance().GetSession().StartGame();
                    GameStateManager.GetInstance().ChangeState(new GameplayState());
                }else{
                    //Nothing
                }
            }else{
                GameStateManager.GetInstance().ChangeState(new GameplayState());
            }
        }

        private void CheckLocalJoins()
        {
            for (int i = 0; i < PlayerManager.MAX_LOCAL_PLAYERS; ++i)
            {
                if (ControllerManager.GetInstance().isPressed(i, "join") && !PlayerManager.GetInstance().IsJoinedLocalPlayer(i))
                {
                    Console.WriteLine("Add Player");
                    PlayerManager.GetInstance().CreatePlayer("Player" + i, true, false, i);
                    TextComponent comp = new TextComponent(100, 150 + players.Count * 50, "Player" + i);
                    players.Add(comp);
                    menu.AddComponent(comp);
                }
            }
        }

        private void CheckNetworkJoins()
        {
        }

        private void UpdateList()
        {
        }

        public override void Draw(GameTime GameTime)
        {
            base.Draw(GameTime);
        }

        public override void Exit()
        {
            Gui.GetInstance().RemoveComponent(menu);
            base.Exit();
        }
    }
}
