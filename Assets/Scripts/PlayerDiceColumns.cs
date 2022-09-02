public class PlayerDiceColumns : DiceColumns
{
    public void SetCanSelectColumn(bool canSelect)
    {
        foreach (DiceColumn column in _columns)
        {
            column.SetIsSelectable(canSelect);
        }
    }
}
