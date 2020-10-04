using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class TransformInfo {

    public Vector3 position;
    public Quaternion rotation;
}

public class LSystemScript : MonoBehaviour
{
    [SerializeField] private float angle = 25f;
    [SerializeField] private float length = 0.6f;
    [SerializeField] private GameObject Branch;
    private const string axiom = "X";
    private Stack<TransformInfo> transformStack;
    private Dictionary<char, string> rules;
    private Dictionary<char, string> rules2;
    private string currentString = string.Empty;
    System.Random rand = new System.Random();

    private GameObject test;
    // Start is called before the first frame update
    void Start()
    {
        test = new GameObject("TEST");
        transformStack = new Stack<TransformInfo>();

        rules = new Dictionary<char, string> {

            {'X', "F+[-F-XF-X][+FF][-XF[+X]][++F-X]"}, // TREE VARIANTE 1
            {'F', "FF"} // TREE VARIANTE 1
            
        };

        rules2 = new Dictionary<char, string> {

            {'X', "FF[+XZ++X-F[+ZX]][-X++F-X]"}, // TREE VARIANTE 2
            {'F', "FX[FX[+XF]]"} // TREE VARIANTE 2
            
        };

        //spawnTest();

        GenerateTree();
    }

    private void spawnTest() {
        
        for(int i = 0; i < 3; i++) {
            int z = rand.Next(-20,20);
            int x = rand.Next(-20,20);
            Instantiate(test, new Vector3(x, 13, z), Quaternion.identity);
        }
    }

    private void GenerateTree() {

        currentString = axiom; // String beginnt mit Regel X

        StringBuilder sb = new StringBuilder();

        int randonNumber = rand.Next(1,10);

        if (randonNumber <= 5) {

            for (int i = 0; i < 3; i++) {
                
                foreach (char c in currentString) {

                    sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
                }

                currentString = sb.ToString();
                sb = new StringBuilder();
                Debug.Log("VARIANTE 1");
                Debug.Log(randonNumber);
            }

        } else if (randonNumber > 5) {
            
            for (int i = 0; i < 3; i++) {
                
                foreach (char c in currentString) {

                    sb.Append(rules2.ContainsKey(c) ? rules2[c] : c.ToString());
                }

                currentString = sb.ToString();
                sb = new StringBuilder();
                Debug.Log("VARIANTE 2");
                Debug.Log(randonNumber);
            }
        }

        foreach( char c in currentString) {

            switch (c) {

                case 'F':
                    Vector3 currentPosition = transform.position;
                    transform.Translate(Vector3.up * length);
                    

                    GameObject Ast = Instantiate(Branch);
                    Ast.GetComponent<LineRenderer>().SetPosition(0, currentPosition);
                    Ast.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                    break;

                case 'X':
                    break;
                case '+':
                    transform.Rotate(Vector3.back * angle);
                    break;
                case '-':
                    transform.Rotate(Vector3.forward * angle);
                    break;
                case '[':
                    transformStack.Push(new TransformInfo() {

                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;

                case ']':
                    TransformInfo ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;           
            }
        }
    }

}
