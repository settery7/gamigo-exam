using System.Collections.Generic;
using TestTask.NonEditable;
using UnityEngine;

namespace TestTask.Editable
{
    public static class ServerPacketsHandler
    {
        #region Packet Handlers
        public static void LoginRequest(Packet packet)
        {
            var clientLogInResponse = ServerMock.Instance.TryConnectClient(out var clientId);
            SendLoginResponse(clientLogInResponse, clientId);

            if (clientLogInResponse == LoginResponse.Success)
                SendMonsterSpawn(ServerMock.Instance.ServerMobsManager.MonsterData);
        }

        public static void MonsterDamageRequest(Packet packet)
        {
            int monsterId = packet.ReadInt();
            float damage = packet.ReadFloat();

            var currentMonster = ServerMock.Instance.ServerMobsManager.MonsterData;
            if (currentMonster == null || currentMonster.MonsterId != monsterId)
                return;

            currentMonster.TakeDamage(damage);
            SendMonsterSpawn(ServerMock.Instance.ServerMobsManager.MonsterData);
        }

        public static void ColorRequest(Packet packet)
        {
            var colors = ServerMock.Instance.ServerColors.GetServerColors();
            SendColorResponse(colors);
        }

        #endregion

        #region Packet Senders
        public static void SendLoginResponse(LoginResponse response, int clientId)
        {
            using (Packet packet = new Packet(1))
            {
                packet.Write((int)response);
                packet.Write(clientId);

                ServerMock.Instance.PacketSenderServer.SendToClient(packet);
            }
        }

        public static void SendColorResponse(IEnumerable<Color> colors)
        {
            using (Packet packet = new Packet(5))
            {
                var colorList = new List<Color>(colors);
                packet.Write(colorList.Count);
                foreach (var color in colorList)
                {
                    packet.Write(color.r);
                    packet.Write(color.g);
                    packet.Write(color.b);
                }
                ServerMock.Instance.PacketSenderServer.SendToClient(packet);
            }
        }

        public static void SendMonsterSpawn(MonsterData monsterData)
        {
            using (Packet packet = new Packet(2))
            {
                packet.Write(monsterData.MonsterId);
                packet.Write((int)monsterData.MonsterType);
                packet.Write(monsterData.MonsterMaxHealth);
                packet.Write(monsterData.MonsterCurrentHealth);

                ServerMock.Instance.PacketSenderServer.SendToClient(packet);
            }
        }

        #endregion
    }
}

public enum LoginResponse
{
    Success = 0,
    Failure = 1,
}