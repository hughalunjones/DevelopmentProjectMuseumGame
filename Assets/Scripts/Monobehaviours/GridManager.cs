using UnityEngine;

public class GridManager : MonoBehaviour {
    [SerializeField] private float rows = 5.0f;
    [SerializeField] private float cols = 5.0f;
    [SerializeField] private int layers = 3;
    [SerializeField] int tileSize = 1;
    public GameObject[] artefactArray;
    public GameObject artefactPrefab;
    public Camera cam;
    int sceneIndex;

    void Start() {
        sceneIndex = GameManager.Instance.GetExcavationSceneIndex();
        GenerateBackground();
        GenerateGrid();
        int randomNum = Random.Range(1, 20);
        if(randomNum != 1) {
            PlaceArtefact();
        }        
    }

    void GenerateBackground() {
        GameObject referenceBgDesert = (GameObject)Instantiate(Resources.Load("DesertDigSite"));
        GameObject referenceBgPermafrost = (GameObject)Instantiate(Resources.Load("PermafrostDigSite"));
        GameObject referenceBgForest = (GameObject)Instantiate(Resources.Load("ForestDigSite"));
        if (sceneIndex == 1) {
            GameObject background = (GameObject)Instantiate(referenceBgDesert, transform);
            background.transform.position = new Vector3(0, 3.4f, 0);
        }
        else if (sceneIndex == 2) {
            GameObject background = (GameObject)Instantiate(referenceBgPermafrost, transform);
            background.transform.position = new Vector3(0, 3.4f, 0);
        }
        else if (sceneIndex == 3) {
            GameObject background = (GameObject)Instantiate(referenceBgForest, transform);
            background.transform.position = new Vector3(0, 3.4f, 0);
        }
        Destroy(referenceBgDesert);
        Destroy(referenceBgPermafrost);
        Destroy(referenceBgForest);
    }
    void GenerateGrid() {
        if (sceneIndex == 1) {
            // Load the reference tiles for the dig spots
            GameObject referenceTileOneDirt = (GameObject)Instantiate(Resources.Load("DirtTileOne"));
            GameObject referenceTileTwoDirt = (GameObject)Instantiate(Resources.Load("DirtTileTwo"));
            GameObject referenceTileThreeDirt = (GameObject)Instantiate(Resources.Load("DirtTileThree"));
            GameObject referenceTileRockSand = (GameObject)Instantiate(Resources.Load("RockTileOne"));
            Grid(referenceTileOneDirt, referenceTileTwoDirt, referenceTileThreeDirt, referenceTileRockSand);
            Destroy(referenceTileOneDirt);
            Destroy(referenceTileTwoDirt);
            Destroy(referenceTileThreeDirt);
            Destroy(referenceTileRockSand);
        }
        if (sceneIndex == 2) {
            // Load the reference tiles for the dig spots
            GameObject referenceTileOneFrost = (GameObject)Instantiate(Resources.Load("FrostTileOne"));
            GameObject referenceTileTwoFrost = (GameObject)Instantiate(Resources.Load("FrostTileTwo"));
            GameObject referenceTileThreeFrost = (GameObject)Instantiate(Resources.Load("FrostTileThree"));
            GameObject referenceTileRockFrost = (GameObject)Instantiate(Resources.Load("FrostTileRock"));
            Grid(referenceTileOneFrost, referenceTileTwoFrost, referenceTileThreeFrost, referenceTileRockFrost);
            Destroy(referenceTileOneFrost);
            Destroy(referenceTileTwoFrost);
            Destroy(referenceTileThreeFrost);
            Destroy(referenceTileRockFrost);
        }
        if (sceneIndex == 3) {
            // Load the reference tiles for the dig spots
            GameObject referenceTileOneSoil = (GameObject)Instantiate(Resources.Load("SoilTileOne"));
            GameObject referenceTileTwoSoil = (GameObject)Instantiate(Resources.Load("SoilTileTwo"));
            GameObject referenceTileThreeSoil = (GameObject)Instantiate(Resources.Load("SoilTileThree"));
            GameObject referenceTileRockSoil = (GameObject)Instantiate(Resources.Load("RockTileTwo"));
            Grid(referenceTileOneSoil, referenceTileTwoSoil, referenceTileThreeSoil, referenceTileRockSoil);
            Destroy(referenceTileOneSoil);
            Destroy(referenceTileTwoSoil);
            Destroy(referenceTileThreeSoil);
            Destroy(referenceTileRockSoil);
        }
    }
    // Place the artefact within the confines of the grid.
    void PlaceArtefact() {
        // Random numbers between the artefact array positions.
        if (sceneIndex == 1) {
            artefactPrefab = artefactArray[Random.Range(5, 12)];
        }
        else if (sceneIndex == 2) {
            artefactPrefab = artefactArray[Random.Range(0, 5)];
        }
        else if (sceneIndex == 3) {
            artefactPrefab = artefactArray[Random.Range(12, 14)];
        }
        Vector3 artefactPos = new Vector3(Random.Range((float)(this.transform.position.x), (float)(this.transform.position.x + 4f)), Random.Range((float)(this.transform.position.y), (float)(this.transform.position.y - 4f)), 1.8f);
        // Random rotation
        GameObject artefact = (GameObject)Instantiate(artefactPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));        
        artefact.transform.position = artefactPos;
        Debug.Log("[GridManager] Artefact placed in grid");
        artefact.transform.SetParent(transform);
    }
    // Random grid generation extracted as its own method due to multiple usage.
    void Grid(GameObject refOne, GameObject refTwo, GameObject refThree, GameObject refRock) {
        for (int layer = 0; layer < layers; layer++) {
            for (float row = 0; row < (rows * refOne.transform.localScale.y); row += refOne.transform.localScale.y) {
                for (float col = 0; col < (cols * refOne.transform.localScale.x); col += refOne.transform.localScale.x) {
                    // Random variable for rock generation
                    int randomRock = Random.Range(1, 8);
                    // Bottom layer is 'undiggable'
                    if (layer == (layers - 1)) {
                        GameObject tile = (GameObject)Instantiate(refThree, transform);
                        tile.GetComponent<DigSpotManager>().isDiggable = false;
                        float posX = this.transform.position.x + (col * tileSize);
                        float posY = this.transform.position.y + (row * -tileSize);
                        tile.transform.position = new Vector3(posX, posY, layer);
                    }
                    else if (layer == 1) {
                        GameObject tile = (GameObject)Instantiate(refTwo, transform);
                        tile.GetComponent<DigSpotManager>().currentHealth = 12;
                        float posX = this.transform.position.x + (col * tileSize);
                        float posY = this.transform.position.y + (row * -tileSize);
                        tile.transform.position = new Vector3(posX, posY, layer);
                    }
                    // Check the rock generation number
                    else if (layer == 0 && randomRock == 1) {
                        GameObject tile = (GameObject)Instantiate(refRock, transform);
                        tile.GetComponent<DigSpotManager>().currentHealth = 12;
                        float posX = this.transform.position.x + (col * tileSize);
                        float posY = this.transform.position.y + (row * -tileSize);
                        tile.transform.position = new Vector3(posX, posY, layer);
                    }
                    else {
                        GameObject tile = (GameObject)Instantiate(refOne, transform);
                        tile.GetComponent<DigSpotManager>().currentHealth = 12;
                        float posX = this.transform.position.x + (col * tileSize);
                        float posY = this.transform.position.y + (row * -tileSize);
                        tile.transform.position = new Vector3(posX, posY, layer);
                    }
                }
            }
        }
    }
}
