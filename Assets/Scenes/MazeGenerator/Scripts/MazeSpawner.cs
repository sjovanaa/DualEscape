using UnityEngine;
using System.Collections;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
    public enum MazeGenerationAlgorithm {
        PureRecursive,
        RecursiveTree,
        RandomTree,
        OldestTree,
        RecursiveDivision,
    }

    public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
    public bool FullRandom = false;
    public int RandomSeed = 12345;
    public GameObject Floor = null;
    public GameObject Wall = null;
    public GameObject Pillar = null;
    public int Rows = 5;
    public int Columns = 5;
    public float CellWidth = 5;
    public float CellHeight = 5;
    public bool AddGaps = true;
    public GameObject GoalPrefab = null;

    private BasicMazeGenerator mMazeGenerator = null;

    void Start() {
        if (!FullRandom) {
            Random.seed = RandomSeed;
        }
        switch (Algorithm) {
            case MazeGenerationAlgorithm.PureRecursive:
            mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
            break;
            case MazeGenerationAlgorithm.RecursiveTree:
            mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
            break;
            case MazeGenerationAlgorithm.RandomTree:
            mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
            break;
            case MazeGenerationAlgorithm.OldestTree:
            mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
            break;
            case MazeGenerationAlgorithm.RecursiveDivision:
            mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
            break;
        }

        mMazeGenerator.GenerateMaze();

        // --- OTVORI PROSTOR OD 20 ĆELIJA U CENTRU ---
        int squareSize = 5; // kvadrat 5x4 = 20 ćelija
        int startRow = (Rows - squareSize + 1) / 2;
        int startCol = (Columns - 4 + 1) / 2;

        for (int row = startRow; row < startRow + 5; row++) {
            for (int col = startCol; col < startCol + 4; col++) {
                MazeCell cell = mMazeGenerator.GetMazeCell(row, col);
                cell.IsGoal = true;

                if (col > startCol) {
                    MazeCell left = mMazeGenerator.GetMazeCell(row, col - 1);
                    cell.WallLeft = false;
                    left.WallRight = false;
                }
                if (row > startRow) {
                    MazeCell top = mMazeGenerator.GetMazeCell(row - 1, col);
                    cell.WallBack = false;
                    top.WallFront = false;
                }
            }
        }

        // --- DODAJ VIŠE ULAZA NA IVICAMA ---
        mMazeGenerator.GetMazeCell(0, Columns / 2).WallBack = false; // gornji ulaz
        mMazeGenerator.GetMazeCell(Rows - 1, Columns / 2).WallFront = false; // donji ulaz

        // --- DODAJ VIŠE PUTEVA DO CENTRA (dodatne rupe u zidovima) ---
        int extraPassages = Mathf.Max(10, (Rows * Columns) / 5); // prilagodi broj dodatnih prolaza prema veličini
        for (int i = 0; i < extraPassages; i++) {
            int r = Random.Range(1, Rows - 2);
            int c = Random.Range(1, Columns - 2);

            MazeCell current = mMazeGenerator.GetMazeCell(r, c);
            MazeCell neighbor = null;

            if (Random.value > 0.5f) {
                if (c + 1 < Columns) {
                    neighbor = mMazeGenerator.GetMazeCell(r, c + 1);
                    current.WallRight = false;
                    neighbor.WallLeft = false;
                }
            }
            else {
                if (r + 1 < Rows) {
                    neighbor = mMazeGenerator.GetMazeCell(r + 1, c);
                    current.WallFront = false;
                    neighbor.WallBack = false;
                }
            }
        }

        // --- INSTANCIJERANJE MAZEA U SCENI ---
        for (int row = 0; row < Rows; row++) {
            for (int column = 0; column < Columns; column++) {
                float x = column * (CellWidth + (AddGaps ? .2f : 0));
                float z = row * (CellHeight + (AddGaps ? .2f : 0));
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
                GameObject tmp;
                tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                tmp.transform.parent = transform;
                if (cell.WallRight) {
                    tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
                    tmp.transform.parent = transform;
                }
                if (cell.WallFront) {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
                    tmp.transform.parent = transform;
                }
                if (cell.WallLeft) {
                    tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
                    tmp.transform.parent = transform;
                }
                if (cell.WallBack) {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
                    tmp.transform.parent = transform;
                }
                if (cell.IsGoal && GoalPrefab != null) {
                    tmp = Instantiate(GoalPrefab, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }

        if (Pillar != null) {
            for (int row = 0; row < Rows + 1; row++) {
                for (int column = 0; column < Columns + 1; column++) {
                    float x = column * (CellWidth + (AddGaps ? .2f : 0));
                    float z = row * (CellHeight + (AddGaps ? .2f : 0));
                    GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }
    }
}
