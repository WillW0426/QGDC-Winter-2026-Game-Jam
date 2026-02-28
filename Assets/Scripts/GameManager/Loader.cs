using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenu,
        LoadingScene,
        TileMapTestScene,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5
    }


    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        if (targetScene == Scene.TileMapTestScene)
        {
            SceneManager.LoadScene("TileMap Test Scene");
        } else
        {
            SceneManager.LoadScene(targetScene.ToString());
        }
    }
}

