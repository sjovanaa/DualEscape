using UnityEngine;

public class KeyCollector : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Key")) {
            Key key = other.GetComponent<Key>();
            if (key != null) {
                string color = key.keyColor;
                Debug.Log("Pokupio ključ boje: " + color);

                foreach (Door door in Door.allDoors) {
                    //Debug.Log("Vrata traže: " + door.requiredKeyColor);

                    if (door.requiredKeyColor == color) {
                        Debug.Log("Otvaram vrata!");
                        door.Open();
                    }
                }

                Destroy(other.gameObject); // Uklanja ključ
            }
        }
    }
}
