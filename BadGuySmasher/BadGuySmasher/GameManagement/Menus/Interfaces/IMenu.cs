namespace BadGuySmasher.GameManagement.Menus.Interfaces
{
  public interface IMenu
  {
    string    Name { get; }

    void      Draw();
    GameState UpdateInput();
  }
}
