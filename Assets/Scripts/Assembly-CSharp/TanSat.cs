﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanSat : Auto
{
	public int templateId;

	public int mobId;

	public override void Run()
	{
		if (Auto.isDie(Char.getMyChar()))
		{
			Hoisinh(true);
			return;
		}
		if (TileMap.mapID != mapID || TileMap.zoneID != zoneID)
		{
			Go(mapID, zoneID);
			return;
		}
		Default();
		PickDa();
		PickHpMp();
		PickNlsk();
		Attack(templateId, mobId);
	}
	public override string ToString()
	{
		if (templateId == -1)
		{
			return "Tàn sát + HS lượng: " + (GameScr.HoiSinhLuong ? "Bật" : "Tắt");
		}
		return "Tàn sát: " + Mob.MobName(templateId) + " + HS lượng: " + (GameScr.HoiSinhLuong ? "Bật" : "Tắt");
	}

	public void update(int temlpateId, int mobId = -1)
	{
		Update();
		zoneID = TileMap.zoneID;
		mapID = TileMap.mapID;
		templateId = temlpateId;
		this.mobId = mobId;
	}
}
