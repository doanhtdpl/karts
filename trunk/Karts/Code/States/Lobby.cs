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
        private NetworkSession session;

        public override void Enter()
        {
            menu = new Screen();
            Gui.GetInstance().AddComponent(menu);

            session = NetworkManager.GetInstance().GetSession();
            if (session != null)
            {
                //session.GamerJoined += new EventHandler<GamerJoinedEventArgs>(session_GamerJoined);
                //session.GamerLeft += new EventHandler<GamerLeftEventArgs>(session_GamerLeft);
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
            }

            UpdateList();
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
                    //NetworkManager.GetInstance().GetSession().StartGame();
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
                    Player newPlayer = PlayerManager.GetInstance().CreatePlayer("Player" + i, true, false, i);

                    if(NetworkManager.GetInstance().HasSession()){
                        NetworkManager.GetInstance().CommunicatePlayerJoined(newPlayer.GetName(), newPlayer.GetID());
                    }
                }
            }
        }

        private void UpdateList()
        {
            menu.RemoveAll();
            int index = 0;
            foreach (Player player in PlayerManager.GetInstance().GetPlayers())
            {
                TextComponent comp = new TextComponent(100, 150 + index * 50, player.GetName());
                players.Add(comp);
                menu.AddComponent(comp);
                index++;
            }

        }

        public override void Exit()
        {
            Gui.GetInstance().RemoveComponent(menu);
            base.Exit();
        }

        void session_GamerJoined(object sender, GamerJoinedEventArgs p)
        {
            menu.AddComponent(new TextComponent(100, 100 * (session.AllGamers.Count + 1), p.Gamer.Gamertag, "KartsFont"));
        }

        void session_GamerLeft(object sender, GamerLeftEventArgs p)
        {
            menu.RemoveAll();
            for (int i = 0; i < session.AllGamers.Count; ++i)
            {
                menu.AddComponent(new TextComponent(100, 100 * (i + 1), session.AllGamers[i].Gamertag, "KartsFont"));
            }
        }
    }
}
