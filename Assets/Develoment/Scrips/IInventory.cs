using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class IInventory : MonoBehaviour
{
    #region listas y arrays
    public List<ISlot> inventory;
    public List<Icraft> crafteos;
    public List<IBuild> buildings;
    public List<string> crafteosUI;
    public ISlot[] MiniInvitario;
    #endregion

    #region UI
    public GameObject MochilaText, InventarioText, CraftText, CofreText, Crafteos;
    public ISlotUI slotUIPrefab, inventarioPrefab, CofrePrefab, CraftingPrefab;
    public Transform MochilaUI, inventarioUI, CraftUI, CofreUI;
    public Image slot1, slot2, slot3, slot4, slot5;
    #endregion

    #region Ubicaciones
    public Transform AparicionMesa, mano,AparicionBuild;
    #endregion

    #region Muerte
     public GameObject player;
    public Transform spawn;
    #endregion

    #region otros
    public int EspacioInv = 0;
    public GameObject camara, inventarioObj, mochilaObj, CraftObj, CofreObj;
    public bool Hacha, Cuchillo, Pico,Comida,botella,agua;
    public GameObject arbol, piedra, Fdog, SPersona;
    IChest Cofre;
    bool Haycofre;
    float timer = 2f;
    #endregion

    #region start and update
    private void Start()

    {
        UpdateUI();
    }

    private void Update()
    {
        if (timer > -1) timer -= Time.deltaTime;
        if (Physics.Raycast(camara.transform.position, camara.transform.forward, out RaycastHit hit, 10f))
        {
            if (Input.GetMouseButtonDown(1)) {
                if (hit.collider.CompareTag("Libro"))
                {
                    Libro();
                }
                if (hit.collider.CompareTag("EndGame")) SceneManager.LoadScene("Win");
                if (hit.collider.CompareTag("Cofre"))
                {
                    Haycofre = true;
                    Cofre = hit.collider.GetComponent<IChest>();
                    UpdateUI();
                    CofreObj.SetActive(!CofreObj.activeSelf);
                    if (CofreObj.activeSelf == true)
                    {
                        Cofre.abierto();
                        Cursor.lockState = CursorLockMode.Confined;
                        Time.timeScale = 0;
                    }
                    else
                    {
                        Cofre.cerrado();
                        Cursor.lockState = CursorLockMode.Locked;
                        Time.timeScale = 1;
                    }
                }
                if (Comida)
                {
                    IStatus status = FindObjectOfType<IStatus>();
                    if (status.hunger > 10)
                    {
                        status.Comida(10);
                        MiniInvitario[EspacioInv].quantity--;
                    }
                }
                if(botella&& hit.collider.CompareTag("agua"))
                {
                    IWater water = MiniInvitario[EspacioInv].objeto.GetComponent<IWater>();
                    water.Cambio();
                    MiniInvitario[EspacioInv].name = "Agua";
                }
                if (agua)
                {
                    IStatus status = FindObjectOfType<IStatus>();
                    if (status.thirst > 10)
                    {
                        status.bebida(10);
                        MiniInvitario[EspacioInv].quantity--;
                    }
                }
                if (hit.collider.CompareTag("agarrable"))
                {
                    IObject objeto = hit.collider.GetComponentInParent<IObject>();
                    hit.collider.transform.rotation = Quaternion.Euler(mano.rotation.x, Quaternion.identity.y, mano.rotation.y);
                    if (objeto.herramienta)
                    {
                        ISlot i = new ISlot(objeto.name, objeto.description, objeto.quantity, objeto.stackable, objeto.herramienta, objeto.gameObject, objeto.imagen2d);
                        asigninv(i,false);
                        objeto.gameobject.SetActive(false);
                    }
                    else
                    Add(objeto);
                }
                if (hit.collider.CompareTag("mesa"))
               {
                   AparicionMesa = hit.collider.transform.GetChild(0).gameObject.transform;                
                   CraftObj.SetActive(!CraftObj.activeSelf);
                    if (CraftObj.activeSelf == true)
                    {
                        Cursor.lockState = CursorLockMode.Confined;
                        Time.timeScale = 0;
                    }
                    else
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Time.timeScale = 1;
                    }              
                }
                if (hit.collider.CompareTag("construible"))
                {
                  IBuilding i =hit.collider.GetComponent<IBuilding>();
                    building(i);
                }
                }
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.CompareTag("Talar") && Hacha)
                {
                    ILoot i = hit.collider.GetComponent<ILoot>();
                    i.spawnLoot(25);
                    Instantiate(arbol, hit.collider.transform.position,Quaternion.identity);
                }
                if (hit.collider.CompareTag("Picar")&& Pico)
                {
                    ILoot i = hit.collider.GetComponent<ILoot>();
                    i.spawnLoot(25);
                    Instantiate(piedra, hit.collider.transform.position, Quaternion.identity);
                }
                if (hit.collider.CompareTag("Enemy") && Cuchillo)
                {
                    IEnemy i = hit.collider.GetComponent<IEnemy>();
                    i.BajaVida(25);
                    Instantiate(Fdog, hit.collider.transform.position, Quaternion.identity);
                }               
            }
        }
        if (Input.GetKeyDown("e"))
        {
            inventarioObj.SetActive(!inventarioObj.activeSelf);
            if (inventarioObj.activeSelf == true)
                Cursor.lockState = CursorLockMode.Confined;
            else
                Cursor.lockState = CursorLockMode.Locked;
      
            mochilaObj.SetActive(!mochilaObj.activeSelf);
            if (mochilaObj.activeSelf == true)
                Cursor.lockState = CursorLockMode.Confined;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }

        imagen();
        Instaciador();
        herramientas();
    }

    public void Libro()
    {
        Crafteos.SetActive(!Crafteos.activeSelf);
        if (Crafteos.activeSelf == true)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
            if (Input.GetMouseButtonDown(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
        }
    }
    #endregion

    #region UI
    public void UpdateUI()
    {
        //inventario
        foreach (Transform child in MochilaUI) if (child.gameObject != slotUIPrefab.gameObject && child.gameObject != MochilaText) Destroy(child.gameObject);
        GameObject mochila = Instantiate(MochilaText, MochilaUI);
        mochila.SetActive(true);
        foreach (var item in inventory)
        {
            ISlotUI slot = Instantiate(slotUIPrefab.gameObject, MochilaUI).GetComponent<ISlotUI>();
            slot.imagen.sprite = item.imagen2d;
            slot.itemName.text = item.name + " x " + item.quantity;
            slot.delete.onClick.AddListener(() => Remove(item, item.quantity));
            slot.removeOne.onClick.AddListener(() => Remove(item, 1));
            if(item.herramienta)
            slot.agarrar.onClick.AddListener(() => asigninv(item,true));
            else
                slot.agarrar.onClick.AddListener(() => nobuilding());
            slot.gameObject.SetActive(true);
        }

        foreach (Transform child in inventarioUI) if (child.gameObject != inventarioPrefab.gameObject && child.gameObject != InventarioText) Destroy(child.gameObject);
        GameObject Inventario = Instantiate(InventarioText, inventarioUI);
        Inventario.SetActive(true);
        foreach (var item in MiniInvitario)
        {
            if (item.name != "vacio") {
                ISlotUI slot = Instantiate(inventarioPrefab.gameObject, inventarioUI).GetComponent<ISlotUI>();
                slot.imagen.sprite = item.imagen2d;
                slot.itemName.text = item.name + " x " + item.quantity;
                slot.soltar.onClick.AddListener(() => DesAsignInv(item));
                slot.gameObject.SetActive(true);
            }
        }
        
        //crafteo

        foreach (Transform child in CraftUI) if (child.gameObject != CraftingPrefab.gameObject) Destroy(child.gameObject);
        GameObject Prefabs = Instantiate(CraftText, CraftUI);
        Prefabs.SetActive(true);
        Asigncrafting();
        foreach (var item in crafteosUI)
        {
            ISlotUI slot = Instantiate(CraftingPrefab.gameObject, CraftUI).GetComponent<ISlotUI>();
            slot.itemName.text = item;
            slot.craft.onClick.AddListener(() => crafting(item));
            slot.gameObject.SetActive(true);
        }
        // cofre 
        if (Haycofre)
        {
            foreach (Transform child in CofreUI) if (child.gameObject != CofrePrefab.gameObject) Destroy(child.gameObject);
            GameObject Cofrex = Instantiate(CofreText, CofreUI);
            Cofrex.SetActive(true);
            foreach (var item in Cofre.Objetos)
            {           
                IObject i = item.GetComponent<IObject>();
                ISlotUI slot = Instantiate(CofrePrefab.gameObject, CofreUI).GetComponent<ISlotUI>();
                slot.itemName.text = i.name;
                slot.imagen.sprite = i.imagen2d;
                slot.quitarCofre.onClick.AddListener(() => 
                {
                    GameObject o = Instantiate(item);
                    IObject oi = o.GetComponent<IObject>();
                    Add(oi); 
                });
                slot.gameObject.SetActive(true);
            }
        }
    }
    #endregion

    #region Muerte
    public void muerte()
    {
        player.transform.position = spawn.position;
        inventory.Clear();
        foreach (var item in MiniInvitario)
        {
            item.name = "vacio";
            item.quantity = 0;
            item.stackable = false;
            Destroy(item.objeto);
            item.objeto = null;
            item.imagen2d = null;
        }
    }
    #endregion

    #region Recorrer Lista


    public void Add(IObject item)
    {
        var newSlot = new ISlot(item.name, item.description, item.quantity, item.stackable,item.herramienta, item.gameobject, item.imagen2d);
        item.gameobject.SetActive(false);

        if (Find(item.name) == null || !Find(item.name).stackable)
        {
            inventory.Add(newSlot);
        }
        else
        {
            Find(item.name).quantity += item.quantity;
        }
        UpdateUI();
    }

    public ISlot Find(string name) => inventory.Find((item) => item.name == name);

    public void Remove(ISlot slot, int quantity)
    {


        if (slot.quantity - quantity <= 0)
        {
            inventory.Remove(Find(slot.name));
            Destroy(slot.objeto);
        }
        else
            slot.quantity -= quantity;

        UpdateUI();
    }
    #endregion

    #region MiniInventario

    void Instaciador()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (EspacioInv == 0) EspacioInv = 4;
            else EspacioInv--;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (EspacioInv == 4) EspacioInv = 0;
            else EspacioInv++;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) EspacioInv = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) EspacioInv = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) EspacioInv = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) EspacioInv = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) EspacioInv = 4;

        for (int i = 0; i < MiniInvitario.Length; i++)
        {
            if (!(i == EspacioInv) && MiniInvitario[i].objeto != null)
                MiniInvitario[i].objeto.SetActive(false);

        }
        if (MiniInvitario[EspacioInv].objeto != null)
        {
            MiniInvitario[EspacioInv].objeto.transform.position = mano.position;
            MiniInvitario[EspacioInv].objeto.transform.SetParent(mano);
            MiniInvitario[EspacioInv].objeto.SetActive(true);
            if (MiniInvitario[EspacioInv].quantity == 0)
            {
                MiniInvitario[EspacioInv].name = "vacio";
                MiniInvitario[EspacioInv].quantity = 0;
                MiniInvitario[EspacioInv].stackable = false;
                MiniInvitario[EspacioInv].objeto.SetActive(false);
                Destroy(MiniInvitario[EspacioInv].objeto);
                MiniInvitario[EspacioInv].imagen2d = null;
            }
        }        
    }

    void herramientas()
    {
        switch (MiniInvitario[EspacioInv].name)
        {
            case "Hacha":
                {
                    Hacha = true;
                    Cuchillo = false;
                    Pico = false;
                    Comida = false;
                    agua = false;
                    botella = false;
                }
                break;
            case "Cuchillo":
                {
                    Hacha = false;
                    Cuchillo = true;
                    Pico = false;
                    Comida = false;
                    agua = false;
                    botella = false;
                }
                break;
            case "Pico":
                {
                    Hacha = false;
                    Cuchillo = false;
                    Pico = true;
                    Comida = false;
                    agua = false;
                    botella = false;
                }
                break;
            case "Manzana":
                {
                    Hacha = false;
                    Cuchillo = false;
                    Pico = false;
                    Comida = true;
                    agua = false;
                    botella = false;
                }

                break;
            case "Naranja":
                {
                    Hacha = false;
                    Cuchillo = false;
                    Pico = false;
                    Comida = true;
                    agua = false;
                    botella = false;
                }
                break;
            case "Botella":
                {
                    Hacha = false;
                    Cuchillo = false;
                    Pico = false;
                    Comida = false;
                    agua = false;
                    botella = true;
                }

                break;
            case "Agua":
                {
                    Hacha = false;
                    Cuchillo = false;
                    Pico = false;
                    Comida = false;
                    agua = true;
                    botella = false;
                }

                break;
            default:
                {
                    Hacha = false;
                    Cuchillo = false;
                    Pico = false;
                    Comida = false;
                    agua = false;
                    botella = false;
                }
                break;
        }
    }
    void asigninv(ISlot slot, bool inventario)
    {
        bool asigno = false;
        for (int i = 0; i < MiniInvitario.Length; i++)
        {
            if (MiniInvitario[i].name == slot.name)
            {
                if (slot.stackable) MiniInvitario[i].quantity++;
                else print("no se staquea");
                asigno = true;
                if(inventario)inventory.Remove(Find(slot.name));
                break;
            }
           else if (MiniInvitario[i].name == "vacio")
            {
                MiniInvitario[i] = slot;
                asigno = true;
                if (inventario) inventory.Remove(Find(slot.name));
                break;
            }
        }
        if (!asigno) print("inventario lleno");

        UpdateUI();
    }
    void DesAsignInv(ISlot slot)
    {
        var newslot = new ISlot(slot.name, slot.description, slot.quantity, slot.stackable,slot.herramienta, slot.objeto, slot.imagen2d);
        inventory.Add(newslot);
        for (int i = 0; i < MiniInvitario.Length; i++)
        {
            if (MiniInvitario[i] == slot)
            {
                MiniInvitario[i].name = "vacio";
                MiniInvitario[i].quantity = 0;
                MiniInvitario[i].stackable = false;
                MiniInvitario[i].objeto.SetActive(false);
                MiniInvitario[i].objeto = null;
                MiniInvitario[i].imagen2d = null;
            }
        }
        UpdateUI();
    }
    void imagen()
    {
        slot1.sprite = MiniInvitario[0].imagen2d;
        slot2.sprite = MiniInvitario[1].imagen2d;
        slot3.sprite = MiniInvitario[2].imagen2d;
        slot4.sprite = MiniInvitario[3].imagen2d;
        slot5.sprite = MiniInvitario[4].imagen2d;
    }

    #endregion

    #region building
    void building(IBuilding building)
    {
        foreach (var item in building.Requisitos)
        {
            foreach (var i in inventory)
            {
                if (i.name == item.name)
                {
                    building.RequisitosCumplidos.Add(item.name);
                    Remove(i, 1);
                    break; 
                }
            }

        }
    }
    void nobuilding()
    {
        if (timer < 0)
        {
            Instantiate(SPersona, transform);
            timer = 2;
        }
    }
        #endregion

    #region crafting 
        void Asigncrafting()
    {
        crafteosUI.Clear();
        foreach (var i in crafteos)
        {
            bool objeto1 = false;
            bool objeto2 = false;
            foreach (var item in inventory)
            {
                if(item.name == i.Objeto1) objeto1= true;
                if(item.name == i.Objeto2) objeto2= true;
            }
            if (objeto1 && objeto2)
                crafteosUI.Add(i.crafteo);
        }
    }
    void crafting(string crafteo)
    {
        foreach (var item in crafteos)
            if (item.crafteo == crafteo)
            {
                for (int i = 0; i < inventory.Count; i++)
                    if (inventory[i].name == item.Objeto1) Remove(inventory[i], 1);

                for (int i = 0; i < inventory.Count; i++)
                    if (inventory[i].name == item.Objeto2) Remove(inventory[i], 1);

                Instantiate(item.crafteoObj, AparicionMesa.position, Quaternion.identity);
            }
        

        UpdateUI();
    }
    #endregion

    #region Islot
    [System.Serializable]
    public class ISlot
    {
        public string name, description;
        public int quantity;
        public bool stackable, herramienta;
        public GameObject objeto;
        public Sprite imagen2d;

        public ISlot(string name, string description, int quantity, bool stackable,bool herramienta, GameObject objeto, Sprite imagen2d)
        {
            this.name = name;
            this.description = description;
            this.quantity = quantity;
            this.stackable = stackable;
            this.herramienta = herramienta;
            this.objeto = objeto;
            this.imagen2d = imagen2d;
        }
    }
    #endregion

    #region Icraft
    [System.Serializable]
    public class Icraft
    {
        public string crafteo, Objeto1, Objeto2;
        public GameObject crafteoObj;

        public Icraft(string Objeto1, string Objeto2, GameObject crafteoObj)
        {
            this.Objeto1 = Objeto1;
            this.Objeto2 = Objeto2;         
            this.crafteoObj = crafteoObj;          
        }
    }
    #endregion

    #region IBuild
    [System.Serializable]
    public class IBuild
    {       
        public string crafteo;
        public GameObject BuildObj;

        public IBuild(string crafteo, GameObject BuildObj)
        {

            this.crafteo = crafteo;
            this.BuildObj = BuildObj;          
        }
    }
    #endregion
}