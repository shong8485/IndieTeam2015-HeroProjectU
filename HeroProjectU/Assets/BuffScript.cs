using UnityEngine;
using System.Collections;

public class BuffScript : MonoBehaviour
{
	public buffTypes buffType;
	public buffAffects buffAffect;
	public float buffDuration;

	public float buffMaxValue;
	public float buffMinValue;
	public float buffTimeMax;
	public float buffTime;

	public float buffStart()
	{
		if (buffAffect == buffAffects.start)
		{
			return getBuffValue();
		}
	}

	public float buffTick()
	{
		if (buffAffect == buffAffects.tick)
		{
			return getBuffValue();
		}
	}

	public float buffEnd()
	{
		if (buffAffect == buffAffects.end)
		{
			return getBuffValue();
		}
	}


	public void updateBuffTime()
	{
		buffTime = buffTime - Time.deltaTime;
	}

	public float getBuffValue()
	{
		return buffMaxValue - (buffMaxValue - buffMinValue) * buffTime / buffTimeMax;
	}

	public enum buffTypes
	{
		defenseBuff,
		attackBuff,
		movespeedBuff,
		healBuff
	}
	public enum buffAffects
	{
		start,
		tick,
		end
	}
}
