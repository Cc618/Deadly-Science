# Reseau

Voici comment fonctionne le reseau en jeu :

Chaque joueur a acces a la classe PlayerNetwork.
Elle comporte PlayerNetwork.localId qui est l'id du joueur actuellement
controlle (le client). (static)
Chaque instance possede PlayerNetwork.id qui est juste l'id de celui
qui le controle et isLocal qui est true si id == localId.

Cette classe se divise en plusieurs parties, la derniere est consacree
a tous les evenements reseau, cad une fonction qui permet de mettre a jour
des parametres en reseau, voici comment en faire un :

### Premiere fonction, la fonction appelee pour chaque joueur

```C#
// From est l'id du joueur qui l'envoie et msg un autre arg
[PunRPC]
public void TestEvent(int from, string msg)
{
    // Traitement, si on veut que seulement le
    // joueur qui envoie l'event soit change,
    // on peut ajouter if (from == id)
    Debug.Log($"NET : Player {from} says : '{msg}'");
}
```

### Deuxieme fonction, celle qui envoie le message par le reseau

```C#
// Les args peuvent etre differents
public void SendTestEvent(string msg)
{
    // id et msg sont les args de TestEvent (fonction 1)
    PhotonView.Get(this).RPC("TestEvent", RpcTarget.All, id, msg);
}
```

Par convention, on met from en premier et les fonctions sont Send<Name> et <Name>
