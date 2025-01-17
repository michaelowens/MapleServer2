﻿using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class PetSkinPacket
{
    private enum PetSkinPacketMode : byte
    {
        Skin = 0x0
    }

    public static PacketWriter Extract(long petUid, Item badge)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PetSkin);
        pWriter.Write(PetSkinPacketMode.Skin);
        pWriter.WriteLong(petUid);
        pWriter.WriteLong(badge.Uid);
        pWriter.WriteInt(badge.PetSkinBadgeId);
        return pWriter;
    }
}
