﻿using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class StatPointHandler : GamePacketHandler<StatPointHandler>
{
    public override RecvOp OpCode => RecvOp.StatPoint;

    private enum StatPointMode : byte
    {
        Increment = 0x2,
        Reset = 0x3
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        StatPointMode mode = (StatPointMode) packet.ReadByte();

        switch (mode)
        {
            case StatPointMode.Increment:
                HandleStatIncrement(session, packet);
                break;
            case StatPointMode.Reset:
                HandleResetStatDistribution(session);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleStatIncrement(GameSession session, PacketReader packet)
    {
        StatAttribute statTypeIndex = (StatAttribute) packet.ReadByte();

        session.Player.StatPointDistribution.AddPoint(statTypeIndex);
        session.Player.FieldPlayer.ComputeStats();
        session.Send(StatPointPacket.WriteStatPointDistribution(session.Player));
        session.Send(StatPacket.SetStats(session.Player.FieldPlayer));
    }

    private static void HandleResetStatDistribution(GameSession session)
    {
        session.Player.StatPointDistribution.ResetPoints();
        session.Player.FieldPlayer.ComputeStats();
        session.Send(StatPointPacket.WriteStatPointDistribution(session.Player));
        session.Send(StatPacket.SetStats(session.Player.FieldPlayer));
    }
}
