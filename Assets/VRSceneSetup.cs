using UnityEngine;

/// <summary>
/// SceneSetup — Gera automaticamente os 5+ objetos exigidos pelo projeto
/// e configura a skybox do ambiente externo (parque/floresta/cidade).
/// 
/// Como usar:
///   1. Crie um GameObject vazio chamado "SceneManager".
///   2. Anexe este script a ele.
///   3. Pressione Play — os objetos serão criados na cena.
///   (Para manter os objetos no Editor, clique em "Set Up Scene" pelo Inspector
///    ou chame SceneSetup.BuildScene() em Start via outro script.)
/// </summary>
public class SceneSetup : MonoBehaviour
{
    [Header("Skybox")]
    [Tooltip("Material de Skybox (arraste um da Asset Store ou deixe null para usar a padrão)")]
    public Material skyboxMaterial;

    [Header("Materiais dos objetos (opcional)")]
    public Material groundMaterial;
    public Material treeTrunkMaterial;
    public Material treeLeaveMaterial;
    public Material benchMaterial;
    public Material lampPostMaterial;
    public Material rockMaterial;

    void Start()
    {
        ApplySkybox();
        BuildScene();
    }

    /// <summary>Aplica o skybox ao ambiente, se houver material configurado.</summary>
    private void ApplySkybox()
    {
        if (skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;
            DynamicGI.UpdateEnvironment();
        }
    }

    /// <summary>Cria todos os elementos da cena de parque ao redor da origem.</summary>
    public void BuildScene()
    {
        CreateGround();
        CreateTrees();
        CreateBench();
        CreateLampPost();
        CreateRocks();
        CreateDirectionalLight();
    }

    // ──────────────────────────────────────────────
    // 1. CHÃO
    // ──────────────────────────────────────────────
    private void CreateGround()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground_Park";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(10f, 1f, 10f); // 100 x 100 unidades

        if (groundMaterial != null)
            ground.GetComponent<Renderer>().material = groundMaterial;
        else
            ApplyColor(ground, new Color(0.2f, 0.5f, 0.15f)); // verde grama

        SetParent(ground, "Environment");
    }

    // ──────────────────────────────────────────────
    // 2. ÁRVORES (3 unidades)
    // ──────────────────────────────────────────────
    private void CreateTrees()
    {
        Vector3[] positions = {
            new Vector3( 8f, 0f,  8f),
            new Vector3(-8f, 0f,  6f),
            new Vector3( 5f, 0f, -9f)
        };

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject tree = new GameObject($"Tree_{i + 1}");
            tree.transform.position = positions[i];

            // Tronco
            GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            trunk.name = "Trunk";
            trunk.transform.SetParent(tree.transform);
            trunk.transform.localPosition = new Vector3(0f, 1f, 0f);
            trunk.transform.localScale = new Vector3(0.3f, 1f, 0.3f);
            if (treeTrunkMaterial != null)
                trunk.GetComponent<Renderer>().material = treeTrunkMaterial;
            else
                ApplyColor(trunk, new Color(0.4f, 0.25f, 0.1f)); // marrom

            // Copa
            GameObject leaves = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            leaves.name = "Leaves";
            leaves.transform.SetParent(tree.transform);
            leaves.transform.localPosition = new Vector3(0f, 2.8f, 0f);
            leaves.transform.localScale = new Vector3(2f, 2f, 2f);
            if (treeLeaveMaterial != null)
                leaves.GetComponent<Renderer>().material = treeLeaveMaterial;
            else
                ApplyColor(leaves, new Color(0.1f, 0.45f, 0.1f)); // verde escuro

            SetParent(tree, "Props/Trees");
        }
    }

    // ──────────────────────────────────────────────
    // 3. BANCO DO PARQUE
    // ──────────────────────────────────────────────
    private void CreateBench()
    {
        GameObject bench = new GameObject("Bench_Park");
        bench.transform.position = new Vector3(3f, 0f, 3f);
        bench.transform.rotation = Quaternion.Euler(0f, 45f, 0f);

        // Assento
        GameObject seat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        seat.name = "Seat";
        seat.transform.SetParent(bench.transform);
        seat.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        seat.transform.localScale = new Vector3(1.8f, 0.1f, 0.5f);

        // Encosto
        GameObject back = GameObject.CreatePrimitive(PrimitiveType.Cube);
        back.name = "Back";
        back.transform.SetParent(bench.transform);
        back.transform.localPosition = new Vector3(0f, 0.8f, -0.22f);
        back.transform.localScale = new Vector3(1.8f, 0.5f, 0.08f);

        if (benchMaterial != null)
        {
            seat.GetComponent<Renderer>().material = benchMaterial;
            back.GetComponent<Renderer>().material = benchMaterial;
        }
        else
        {
            ApplyColor(seat, new Color(0.55f, 0.35f, 0.15f));
            ApplyColor(back, new Color(0.55f, 0.35f, 0.15f));
        }

        SetParent(bench, "Props/Furniture");
    }

    // ──────────────────────────────────────────────
    // 4. POSTE DE LUZ
    // ──────────────────────────────────────────────
    private void CreateLampPost()
    {
        GameObject lampPost = new GameObject("LampPost_01");
        lampPost.transform.position = new Vector3(-4f, 0f, 4f);

        // Poste
        GameObject pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pole.name = "Pole";
        pole.transform.SetParent(lampPost.transform);
        pole.transform.localPosition = new Vector3(0f, 2.5f, 0f);
        pole.transform.localScale = new Vector3(0.1f, 2.5f, 0.1f);
        ApplyColor(pole, new Color(0.2f, 0.2f, 0.2f));

        // Luminária
        GameObject lamp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        lamp.name = "Lamp";
        lamp.transform.SetParent(lampPost.transform);
        lamp.transform.localPosition = new Vector3(0f, 5.1f, 0f);
        lamp.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        ApplyColor(lamp, new Color(1f, 0.95f, 0.7f));

        // Luz pontual
        GameObject lightObj = new GameObject("PointLight");
        lightObj.transform.SetParent(lampPost.transform);
        lightObj.transform.localPosition = new Vector3(0f, 5f, 0f);
        Light pointLight = lightObj.AddComponent<Light>();
        pointLight.type = LightType.Point;
        pointLight.color = new Color(1f, 0.95f, 0.7f);
        pointLight.intensity = 2f;
        pointLight.range = 10f;

        if (lampPostMaterial != null)
            pole.GetComponent<Renderer>().material = lampPostMaterial;

        SetParent(lampPost, "Props/Lights");
    }

    // ──────────────────────────────────────────────
    // 5. PEDRAS
    // ──────────────────────────────────────────────
    private void CreateRocks()
    {
        Vector3[] positions = {
            new Vector3(-6f, 0.2f, -5f),
            new Vector3(-7f, 0.15f, -3f),
            new Vector3( 9f, 0.2f, -2f)
        };
        Vector3[] scales = {
            new Vector3(0.8f, 0.4f, 0.6f),
            new Vector3(0.5f, 0.3f, 0.5f),
            new Vector3(1.0f, 0.5f, 0.8f)
        };

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject rock = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            rock.name = $"Rock_{i + 1}";
            rock.transform.position = positions[i];
            rock.transform.localScale = scales[i];
            rock.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            if (rockMaterial != null)
                rock.GetComponent<Renderer>().material = rockMaterial;
            else
                ApplyColor(rock, new Color(0.5f, 0.5f, 0.5f));

            SetParent(rock, "Props/Rocks");
        }
    }

    // ──────────────────────────────────────────────
    // LUZ DIRECIONAL (sol)
    // ──────────────────────────────────────────────
    private void CreateDirectionalLight()
    {
        // Verifica se já existe uma luz direcional
        if (FindObjectOfType<Light>() != null) return;

        GameObject sunObj = new GameObject("Sun_DirectionalLight");
        sunObj.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        Light sun = sunObj.AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.2f;
        sun.color = new Color(1f, 0.95f, 0.85f);
        sun.shadows = LightShadows.Soft;

        SetParent(sunObj, "Environment");
    }

    // ──────────────────────────────────────────────
    // HELPERS
    // ──────────────────────────────────────────────

    /// <summary>Aplica uma cor sólida ao Renderer do objeto.</summary>
    private void ApplyColor(GameObject obj, Color color)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = new Material(Shader.Find("Standard"));
            rend.material.color = color;
        }
    }

    /// <summary>Organiza o objeto numa pasta (GameObject pai) na hierarquia.</summary>
    private void SetParent(GameObject obj, string folderPath)
    {
        string[] parts = folderPath.Split('/');
        Transform current = null;

        foreach (string part in parts)
        {
            GameObject found = GameObject.Find(part);
            if (found == null)
            {
                found = new GameObject(part);
                if (current != null)
                    found.transform.SetParent(current);
            }
            current = found.transform;
        }

        if (current != null)
            obj.transform.SetParent(current);
    }
}
