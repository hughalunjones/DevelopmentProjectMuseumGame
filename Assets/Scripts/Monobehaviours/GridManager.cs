using UnityEngine;

public class GridManager : MonoBehaviour {
    [SerializeField] private float rows = 5.0f;
    [SerializeField] private float cols = 5.0f;
    [SerializeField] private int layers = 3;
    [SerializeField] int tileSize = 1;
    public GameObject[] artefactArray;
    public GameObject artefactPrefab;
    public Camera cam;
    int randomBg;

    void Start() {
        randomBg = Random.Range(1, 3);
        GenerateBackground();
        GenerateGrid();
        PlaceArtefact();
    }
    /* TODO:
     *      - Final background and associated tiles.
     *      - The other 16 artefacts.
     *      - Fix the background positioning - currently hardcoded. 
     *
     */
    void GenerateBackground() {
        GameObject referenceBgDesert = (GameObject)Instantiate(Resources.Load("DesertDigSite"));
        GameObject referenceBgPermafrost = (GameObject)Instantiate(Resources.Load("PermafrostDigSite"));
        if (randomBg == 1 || randomBg == 3) {
            GameObject background = (GameObject)Instantiate(referenceBgDesert, transform);
            background.transform.position = new Vector3(0, 3.4f, 0);
        }
        else if (randomBg == 2) {
            GameObject background = (GameObject)Instantiate(referenceBgPermafrost, transform);
            background.transform.position = new Vector3(0, 3.4f, 0);
        }
        Destroy(referenceBgDesert);
        Destroy(referenceBgPermafrost);
    }
    void GenerateGrid() {
        if (randomBg == 1 || randomBg == 3) {
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
        if (randomBg == 2) {
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
    }
    // Place the artefact within the confines of the grid.
    void PlaceArtefact() {
        // Random numbers between the artefact array positions.
        if (randomBg == 1) {
            artefactPrefab = artefactArray[1];
        }
        else if (randomBg == 2) {
            artefactPrefab = artefactArray[0];
        }
        else if (randomBg == 3) {
            artefactPrefab = artefactArray[1];
        }
        Vector3 artefactPos = new Vector3(Random.Range((float)(this.transform.position.x), (float)(this.transform.position.x + 4f)), Random.Range((float)(this.transform.position.y), (float)(this.transform.position.y - 4f)), 1.8f);
        // Random rotation
        GameObject artefact = (GameObject)Instantiate(artefactPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        artefact.transform.position = artefactPos;
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
