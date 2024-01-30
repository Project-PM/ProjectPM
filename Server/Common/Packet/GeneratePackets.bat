START ../../PacketGenerator/bin/PacketGenerator.exe ../../PacketGenerator/BATTLE_PDL.xml

XCOPY /Y BattleGeneratedPacket.cs "../../../Client/Assets/Scripts/Network/Packet"
XCOPY /Y BattleGeneratedPacket.cs "../../BattleServer/Packet"

XCOPY /Y BattleClientPacketManager.cs "../../../Client/Assets/Scripts/Network/Packet"
XCOPY /Y BattleServerPacketManager.cs "../../BattleServer/Packet"