using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]private int rows = 5;
    [SerializeField]private int cols = 5;
    [SerializeField]private int layers = 3;
    [SerializeField]int tileSize = 1;
    public GameObject artefactPrefab;

    void Start(){
        GenerateGrid();
        PlaceArtefact();
    }
    /* TODO:
     *    - Random background
     *    - Background dictates artefacts
     *    - Different tiles.
     *
     */
    void GenerateGrid() {
        // Loop for the number of layers in the digsite
        for(int layer = 0; layer < layers; layer++) {
            // Load the reference tiles for the dig spots
            GameObject referenceTileOne = (GameObject)Instantiate(Resources.Load("DirtTileOne"));
            GameObject referenceTileTwo = (GameObject)Instantiate(Resources.Load("DirtTileTwo"));
            GameObject referenceTileRock = (GameObject)Instantiate(Resources.Load("RockTileOne"));
            // Loop through the number of columns and rows.
            for (int row = 0; row < rows; row++) {
                for (int col = 0; col < cols; col++) {
                    // Random variable for rock generation
                    int randomRock = Random.Range(1, 8);
                    // Bottom layer is 'undiggable'
                    if (layer == (layers - 1)) {
                        GameObject tile = (GameObject)Instantiate(referenceTileOne, transform);
                        tile.GetComponent<DigSpotManager>().isDiggable = false;
                        float posX = this.transform.position.x + (col * tileSize);
                        float posY = this.transform.position.y + (row * -tileSize);
                        tile.transform.position = new Vector3(posX, posY, layer);
                    }
                    else if (layer == 1) {
                        GameObject tile = (GameObject)Instantiate(referenceTileTwo, transform);
                        tile.GetComponent<DigSpotManager>().currentHealth = 12;
                        float posX = this.transform.position.x + (col * tileSize);
                        float posY = this.transform.position.y + (row * -tileSize);
                        tile.transform.position = new Vector3(posX, posY, layer);
                    }
                    // Check the rock generation number
                    else if (layer == 0 && randomRock == 1){
                        GameObject tile = (GameObject)Instantiate(referenceTileRock, transform);
                        tile.GetComponent<DigSpotManager>().currentHealth = 12;
                        float posX = this.transform.position.x + (col * tileSize);
                        float posY = this.transform.position.y + (row * -tileSize);
                        tile.transform.position = new Vector3(posX, posY, layer);
                    }
                    else {
                        GameObject tile = (GameObject)Instantiate(referenceTileOne, transform);
                        tile.GetComponent<DigSpotManager>().currentHealth = 12;
                        float posX = this.transform.position.x + (col * tileSize);
                        float posY = this.transform.position.y + (row * -tileSize);
                        tile.transform.position = new Vector3(posX, posY, layer);
                    }
                }
            }
            Destroy(referenceTileOne);
            Destroy(referenceTileTwo);
            Destroy(referenceTileRock);
        }
    }
    // Place the artefact within the confines of the grid.
      void PlaceArtefact() {
        Vector3 artefactPos = new Vector3(Random.Range((float)(this.transform.position.x), (float)(this.transform.position.x + 4f)), Random.Range((float)(this.transform.position.y), (float)(this.transform.position.y - 4f)), 1.8f);
        GameObject artefact = (GameObject)Instantiate(artefactPrefab, transform);
        artefact.transform.position = artefactPos;
    }
}
