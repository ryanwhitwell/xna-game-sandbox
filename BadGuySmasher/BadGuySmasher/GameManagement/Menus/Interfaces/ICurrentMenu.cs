namespace BadGuySmasher.GameManagement.Menus.Interfaces
{
  public interface ICurrentMenu : IMenu 
  {
    void SetGameState(GameState gameState);
  }
}
