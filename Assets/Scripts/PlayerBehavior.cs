using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CMF;

[System.Serializable]
public class PlayerSoundList
{
    public AudioClip shoot;
    public AudioClip hurt;
    public AudioClip death;
}

public class PlayerBehavior : MonoBehaviour
{
    [Header("Shooting")]
    public string fireKey = "Fire1";
    public float cooldown = 0.4f;
    bool canShoot = true;
    public AffinityType affinity;

    public PlayerSoundList playerSoundList;
    GameObject projectilePrefab;
    GameObject projectileSpawn;


    [Header("Life")]
    public int health = 100;
    GameObject hitParticles;


    // Private non-specific variables
    string prefabResourcePath = "Prefabs/";
    string particlePrefabPath = "Particles/";
    Transform modelRoot;
    CharacterInput characterInput;
    GameObject playerUI;
    Image healthBar;
    Text healthText;
    GameScript gameScript;

    bool hasGameStarted = false;


    void Awake()
    {
        // Assign private variables
        characterInput = gameObject.GetComponent<CharacterInput>();
        characterInput.enabled = false;
        modelRoot = transform.GetChild(0);
        projectileSpawn = modelRoot.Find("ProjectileSpawn").gameObject;
        gameScript = GameObject.Find("GameMaster").GetComponent<GameScript>();

        // Assign UI
        playerUI = GameObject.Find(characterInput.currentPlayer.ToString() + "_UI");
        healthBar = playerUI.transform.Find("Health Bar").GetComponent<Image>();
        healthText = playerUI.transform.Find("Health Text").GetComponent<Text>();
        UpdateHealthUI();

        // Initialize affinity
        SetupAffinity(AffinityHelpers.PickRandomAffinity());
    }

    void Update()
    {
        if (!hasGameStarted && gameScript.status != GameScript.GameStatus.NotStarted)
        {
            hasGameStarted = true;
            characterInput.enabled = true;
        }

        if (characterInput.IsFireKeyPressed())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (!CanAct() || !canShoot) return;

        PlayAudio(playerSoundList.shoot, 1f);
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.transform.position, projectileSpawn.transform.rotation);
        ProjectileBehavior behavior = projectile.GetComponent<ProjectileBehavior>();
        float childDir = modelRoot.eulerAngles.y;
        Vector3 moveDirection = new Vector3(Mathf.Floor(childDir) > 90 ? -1 : 1, 0, 0);
        behavior.Setup(gameObject, affinity, moveDirection);
        Destroy(projectile, 5);

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    public int TakeDamage(AffinityType type)
    {
        int multi = AffinityHelpers.GetAffinityMultiplierValue(type, affinity);
        int dmg = 1 * multi;
        health -= dmg;
        UpdateHealthUI();

        if (dmg > 0)
        {
            if (Random.Range(0, 9) == 5) PlayAudio(playerSoundList.hurt, 2f);
        }


        if (health <= 0)
        {
            gameScript.EndGame(characterInput.currentPlayer);
            Die();
        }

        return dmg;
    }

    // Holds all logic to make player match affinity. (Projectiles, ui, etc.)
    public void SetupAffinity(AffinityType _affinity)
    {
        affinity = _affinity;

        // Projectile
        projectilePrefab = Resources.Load(prefabResourcePath + _affinity.ToString()) as GameObject;

        // UI
        Transform weaponUI = playerUI.transform.Find("Weapon");
        Image weaponIcon = weaponUI.Find("Icon").GetComponent<Image>();
        Text weaponText = weaponUI.Find("Label").GetComponent<Text>();

        switch (_affinity.ToString())
        {
            case "Rock1":
                weaponIcon.sprite = gameScript.rockIcon;
                weaponText.text = "Rock";
                break;
            case "Paper1":
                weaponIcon.sprite = gameScript.paperIcon;
                weaponText.text = "Paper";
                break;
            case "Scissors1":
                weaponIcon.sprite = gameScript.scissorsIcon;
                weaponText.text = "Scissors";
                break;
            default:
                Debug.LogWarning("Unknown affinity: " + _affinity.ToString());
                break;
        }
    }

    void UpdateHealthUI()
    {
        // Bar
        float newFillAmount = health / 100f;
        healthBar.fillAmount = newFillAmount;

        // Text
        healthText.text = health.ToString();
    }

    void Die()
    {
        PlayAudio(playerSoundList.death);
        GameObject deathParticlesPrefab = Resources.Load(particlePrefabPath + "Death Explosion") as GameObject;
        GameObject deathParticles = Instantiate(deathParticlesPrefab, transform.position, Quaternion.Euler(Vector3.zero));
        Destroy(playerUI);
        Destroy(gameObject);
        Destroy(deathParticles, 3);
    }

    public void PlayAudio(AudioClip clip, float volume = 1)
    {
        gameScript.PlayAudio(clip, volume);
    }

    bool CanAct()
    {
        return hasGameStarted;
    }
}