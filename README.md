# RPA-HIS

## La preuve de concept

Le projet RPA "Ambulance" du MESS devait démontré qu'il était possible d'automatiser des tâches par robotisation.

Pour ce faire différents outils ont été utilisés

- Power Automate
- Du code maison
- HIS 3270 Design Tool
- Le central IBM

````mermaid
sequenceDiagram
    Power Automate->>API Maison .net: Traiter une facture
    Note right of Power Automate: Via Azure On-premise<br>data gateway
    API Maison .net->>+Central: Lancer HDIX (pilote 3270)
    Note right of API Maison .net: Via fichier .HIDX
    Central->>-API Maison .net: Retour du central
    API Maison .net->>Power Automate: Retour du central
````

Sur le diagrame ci-dessus, c'est ``Power automate`` qui lance un fichier ``HIDX`` via un ``API REST maison``, le tout via une passerelle `API Gateway` sur `w700s001app17`.

En temps normal, ``Power Automate`` et l'``API maison`` auraient tout deux pu êtres remplacés simplement par ``Azure Logic Apps`` et le connecteur ``IBM 3270 connector``, par contre il faut alors utiliser un vrai VPN site-to-site avec Azure et nous n'en avons pas encore à ce jour.

## HIS 3270 Design tool

L'outil n'est pas parfait et demande un peu de temps pour le maitriser, surtout parce qu'il est un peu inachevé. Malgré tout, il est capable de produire des plan de navigation.

Dans ce dépot GIT, dans le répertoire [HIS3270_Design_Tool](https://github.com/MTESSDev/RPA-HIS/tree/main/HIS3270_Design_Tool), j'ai laissé le fichier ``.RAP`` qui peut-être ouvert dans HIS Design Tool pour l'éditer et le fichier ``.hidx`` produit par celui-ci en sortie.

Téléchargeable ici:\
https://www.microsoft.com/en-us/download/details.aspx?id=57962

## Documentation et références

HIS Design tool et connecteur Azure Logic Apps :\
https://learn.microsoft.com/en-us/azure/connectors/connectors-run-3270-apps-ibm-mainframe-create-api-3270

Azure On-premise data gateway :\
https://learn.microsoft.com/fr-ca/data-integration/gateway/service-gateway-onprem