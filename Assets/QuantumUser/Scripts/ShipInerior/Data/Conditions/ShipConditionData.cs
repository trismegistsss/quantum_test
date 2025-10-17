using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGS.EditorView.ShipEditor.Data.Conditions
{
    public class ShipConditionData
    {
		private AbstractConditionData loseCondition;

		private List<WinConditionData> winConditions = new List<WinConditionData>();

		private bool finishAfterAllWins = true;

		public AbstractConditionData LoseCondition
		{
			get { return loseCondition; }
			set { loseCondition = value; }
		}

		public List<WinConditionData> WinConditions
		{
			get { return winConditions; }
			set { winConditions = value; }
		}

		public bool FinishAfterAllWins
		{
			get { return finishAfterAllWins; }
			set { finishAfterAllWins = value; }
		}
	}
}