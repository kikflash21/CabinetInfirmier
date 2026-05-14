<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:cab="http://www.univ-grenoble-alpes.fr/l3miage/medical"
                xmlns:act="http://www.univ-grenoble-alpes.fr/l3miage/actes"
                exclude-result-prefixes="cab act"
>
    <xsl:output method="html" indent="yes"/>
    
    <!-- param globale avec l'id de l'infirmier -->
    <xsl:param name="destinedId">001</xsl:param>
    
    <!-- variable globale contenant les noeuds ngap du document actes.xml -->
    <xsl:variable name="actes" select="document('../xml/actes.xml', /)/act:ngap"/>
    
    
    <xsl:template match="/">
        <xsl:variable name="prenomInf" select="./cab:cabinet/cab:infirmiers/cab:infirmier[@id=$destinedId]/cab:prénom"/>
        <xsl:variable name="patients" select="//cab:patient[cab:visite[@intervenant=$destinedId]]"/>
        <html>
            <head>
                <title>page infirmiere</title>
                <link rel="stylesheet" href="../css/pageInfirmier.css"/>
                <!--<script type="text/javascript" src="../js/facture.js"></script> -->
            </head>
            
            <body>
                <div class="classPatient">
                    <h1>Bonjour</h1>
                    <h3><xsl:value-of select="$prenomInf"/></h3>
                    <h1>, aujourd'hui, vous avez</h1>
                    <h3><xsl:value-of select="count($patients)"/></h3>
                    <h1>patients.</h1>
                    <h1>Voici la liste des patients à visiter</h1>
                </div>
                <!-- Liste des patients et des soins -->
                <div>
                    <xsl:apply-templates select="$patients"/>
                </div>
            </body>
            
        </html>
    </xsl:template>
    
    <!-- Template patient -->
    <xsl:template match="cab:patient">
        <div class="classPatient">
            <table>
                <tr><th>Nom</th><td><xsl:value-of select="cab:nom"/></td></tr>
                <tr><th>Prénom</th><td><xsl:value-of select="cab:prénom"/></td></tr>
                <tr><th>Sexe</th><td><xsl:value-of select="cab:sexe"/></td></tr>
                <tr><th>Date de naissance</th><td><xsl:value-of select="cab:naissance"/></td></tr>
                <tr><th>Numéro de sécurité sociale</th><td><xsl:value-of select="cab:numéro"/></td></tr>
                <tr><th>Adresse</th><td><xsl:apply-templates select="cab:adresse"/></td></tr>
            </table>
            
            <!-- Liste des soins -->
            <h2>Liste des soins à effectuer pour ce patient</h2>
            <table>
                <tr><th>Date</th><th>Actes</th></tr>
                <xsl:apply-templates select="cab:visite">
                    <xsl:sort select="@date" order="ascending"/>
                </xsl:apply-templates>
            </table>
            
            <!-- Bouton facture -->
            <h4><u>Facture</u> :</h4>
            <script type="text/javascript">
                function openFacture(prenom, nom, actes) {
                    var width = 500;
                    var height = 300;
                    if (window.innerWidth) {
                        var left = (window.innerWidth-width)/2;
                        var top = (window.innerHeight-height)/2;
                    } else {
                        var left = (document.body.clientWidth-width)/2;
                        var top = (document.body.clientHeight-height)/2;
                    }
                    var factureWindow = window.open('','facture','menubar=yes, scrollbars = yes, top='+top+', left='+left+', width='+width+', height='+height +'');
                    factureText = "Facture pour : " + prenom + " " + nom;
                    factureWindow.document.write(factureText)
                }
            </script>
            <xsl:element name="button">
                <xsl:attribute name="onclick">
                    openFacture('<xsl:value-of select="cab:prénom"/>',
                    '<xsl:value-of select="cab:nom"/>',
                    '<xsl:value-of select="cab:visite/cab:acte"/>')
                </xsl:attribute>
                Facture
            </xsl:element>
        </div>
    </xsl:template>
    
    <!-- Template visites -->
    <xsl:template match="//cab:patient/cab:visite">
        <tr>
            <td><xsl:value-of select="@date"/></td>
            <td><xsl:apply-templates select="cab:acte"/></td>
        </tr>
    </xsl:template>
    
    <!-- Template acte -->
    <xsl:template match="//cab:patient//cab:acte">
        <xsl:variable name="idActe" select="@id"/>
        <li><xsl:value-of select="$actes/act:actes/act:acte[@id=$idActe]/text()"/></li>
    </xsl:template>
    
    <!-- Template adresse -->
    <xsl:template match="cab:adresse">
        <span>
            <!-- on vérifire si l'adresse à un étage -->
            <xsl:if test="cab:étage">
                <xsl:value-of select="cab:étage"/>
                <xsl:text>è étage, </xsl:text>
            </xsl:if>
            <!-- on vérifire si l'adresse à un numéro de rue -->
            <xsl:if test="cab:numéro">
                <xsl:value-of select="cab:numéro"/>
                <xsl:text> </xsl:text>
            </xsl:if>
            <xsl:value-of select="cab:rue"/>
            <xsl:text>, </xsl:text>
            <xsl:value-of select="cab:codePostal"/>
            <xsl:text> </xsl:text>
            <xsl:value-of select="cab:ville"/>
        </span>
    </xsl:template>
    
    <!-- Template pour les nœuds text (pour redefinir le comportement par défaut) -->
    <xsl:template match="text()"/>

    <!-- Template pour les nœuds commentaires (pour redefinir le comportement par défaut) -->
    <xsl:template match="comment()"/>
    
</xsl:stylesheet>