using BadGuySmasher.GameManagement.Menus.Interfaces;

namespace BadGuySmasher.GameManagement.Interfaces
{
  public interface IGameManager
  {
    ILevelManager LevelManager  { get; }
    GameState     GameState     { get; set; }
    IMenuManager  MenuManager   { get; }
  }
}
