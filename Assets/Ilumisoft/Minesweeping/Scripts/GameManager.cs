using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ilumisoft.Minesweeping
{
    public class GameManager : MonoBehaviour, ITileClickListener
    {
        [SerializeField]
        GameObject levelCompleteUI = null;

        [SerializeField]
        GameObject gameOverUI = null;

        [SerializeField]
        Tile normalTilePrefab = null;

        [SerializeField]
        Tile bombTilePrefab = null;

        [SerializeField]
        TileGrid grid = null;

        [SerializeField]
        private int bombCount = 5;

        public int BombCount { get => bombCount; }

        GameObject tileContainer = null;

        List<Tile> tiles = new List<Tile>();

        private void Awake()
        {
            tileContainer = new GameObject("Tile Container");
        }

        void Start()
        {
            Debug.Log($"[GameManager] Starting with bombCount={bombCount}, grid={grid.Width}x{grid.Height}");
            CreateBoard();
        }

        /// <summary>
        /// 
        /// </summary>
        void CreateBoard()
        {
            AddBombsToGrid();
            AddNormalTilesToGrid();
            AssignBombNumbers();
        }

        /// <summary>
        /// Adds the bomb tiles to the grid at 4 rightmost tiles in top row
        /// </summary>
        private void AddBombsToGrid()
        {
            // Make sure the number of bombs is not larger than the grid size
            bombCount = Mathf.Min(bombCount, grid.Width * grid.Height);

            // Calculate random positions for the bombs
            int placed = 0;

            // Place bombs at random unique positions until we've placed bombCount bombs
            while (placed < bombCount)
            {
                int x = Random.Range(0, grid.Width);
                int y = Random.Range(0, grid.Height);

                // If there is already a tile at this position, skip it
                if (grid.TryGetTile(x, y, out _))
                {
                    continue;
                }

                AddTileToGrid(x, y, bombTilePrefab);
                placed++;
            }

            Debug.Log($"[GameManager] Placed {placed} bombs (requested {bombCount})");
        }


        /// <summary>
        /// Allows changing the bomb count at runtime before board creation.
        /// Call this before the scene starts or before `CreateBoard()` is invoked.
        /// </summary>
        /// <param name="count"></param>
        public void SetBombCount(int count)
        {
            bombCount = Mathf.Max(0, count);
        }
        /// <summary>
        /// Fills all empty grid cells with normal tiles
        /// </summary>
        private void AddNormalTilesToGrid()
        {
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    // If no (bomb) tile is already set, add a normal tile
                    if (grid.TryGetTile(x, y, out _) == false)
                    {
                        AddTileToGrid(x, y, normalTilePrefab);
                    }
                }
            }
        }

        /// <summary>
        /// Assigns to each tile the number of surrounding bombs
        /// </summary>
        private void AssignBombNumbers()
        {
            foreach (var tile in tiles)
            {
                if (tile.TryGetComponent<TileNumber>(out var tileNumber))
                {
                    tileNumber.SetNumberOfBombs(grid.GetNumberOfSurroundingBombs(tile));
                }
            }
        }

        /// <summary>
        /// Creates an instance of the given tile prefab and adds it to the given grid position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="prefab"></param>
        void AddTileToGrid(int x, int y, Tile prefab)
        {
            var position = grid.GetWorldPosition(x, y);

            var tile = Instantiate(prefab, position, Quaternion.identity);
            tile.transform.SetParent(tileContainer.transform);
            grid.SetTile(x, y, tile);

            tiles.Add(tile);
        }

        public void OnTileClick(Tile tile)
        {
            if (tile.State == TileState.Hidden)
            {
                TileRevealer tileRevealer = new TileRevealer(grid);

                tileRevealer.Reveal(tile);

                if (tile.CompareTag(Bomb.Tag))
                {
                    GameOver(won: false);
                }
                else if(HasRevealedAllSafeTiles())
                {
                    GameOver(won: true);
                }
            }
        }

        bool HasRevealedAllSafeTiles()
        {
            foreach (var tile in tiles)
            {
                if (tile.CompareTag(Bomb.Tag) == false && tile.State != TileState.Revealed)
                {
                    return false;
                }
            }

            return true;
        }

        void GameOver(bool won)
        {
            StopAllCoroutines();
            StartCoroutine(GameOverCoroutine(won));
        }

        IEnumerator GameOverCoroutine(bool won)
        {
            GameObject uiElement = won ? levelCompleteUI : gameOverUI;

            yield return new WaitForSecondsRealtime(1.0f);

            uiElement.SetActive(true);
        }
    }
}