using System.Collections.Generic;
using TestTask.NonEditable;
using UnityEngine;

namespace TestTask.Editable
{
    public static class ClientPacketsHandler
    {
        #region Packet Handlers
        public static void LoginDataReceived(Packet packet)
        {
            if (ClientManager.Instance == null) return;

            int responseCode = packet.ReadInt();
            int clientId = packet.ReadInt();

            ClientManager.Instance.SetClientLogInStatus(responseCode, clientId);
        }

        public static void MonsterSpawnReceived(Packet packet)
        {
            if (ClientManager.Instance == null) return;

            int monsterId = packet.ReadInt();
            MonsterNames monsterType = (MonsterNames)packet.ReadInt();
            float maxHp = packet.ReadFloat();
            float currentHp = packet.ReadFloat();

            var monsterData = new MonsterData(monsterId, monsterType, maxHp, currentHp);
            ClientManager.Instance.ClientMobsManager.SetMonster(monsterData);
        }

        public static void ColorResponseReceived(Packet packet)
        {
            if (ClientManager.Instance == null) return;

            int count = packet.ReadInt();
            var colors = new List<Color>(count);
            for (int i = 0; i < count; i++)
            {
                float r = packet.ReadFloat();
                float g = packet.ReadFloat();
                float b = packet.ReadFloat();
                colors.Add(new Color(r, g, b, 1f));
            }
            ClientManager.Instance.ClientColorManager.SetColors(colors);
        }
        #endregion

        #region Packet Senders
        public static void SendLoginRequest()
        {
            Packet packet = new Packet(1);
            ClientManager.Instance.PacketSenderClient.SendToServer(packet);
        }

        public static void SendMonsterDamage(int monsterId, float damage)
        {
            using (Packet packet = new Packet(3))
            {
                packet.Write(monsterId);
                packet.Write(damage);
                ClientManager.Instance.PacketSenderClient.SendToServer(packet);
            }
        }

        public static void SendColorRequest()
        {
            using (Packet packet = new Packet(4))
            {
                ClientManager.Instance.PacketSenderClient.SendToServer(packet);
            }
        }
        #endregion
    }
}
