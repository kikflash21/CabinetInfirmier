<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:pat="http://www.univ-grenoble-alpes.fr/l3miage/patient"
                version="1.0"
                exclude-result-prefixes="pat"
>
    <xsl:output method="html" indent="yes"/>
    
    <xsl:template match="/">
        <html>
            <head>
                <title>page patient</title>
                <link href="../css/pagePatient.css" rel="stylesheet"/>
            </head>
            <body>
                <div class="classPatient">
                    <h1>Bonjour</h1>
                    <h3><xsl:value-of select="pat:patient/pat:prénom"/></h3>
                    <h1> :)</h1>
                </div>
                <div class="classPatient">
                    <xsl:apply-templates select="pat:patient"/>
                </div>
            </body>
        </html>
    </xsl:template>
    
    <!-- Il devrait y avoir, noremalement, un seul match pour cette template (la racine patient) -->
    <xsl:template match="pat:patient">
        <h2>Infos générales</h2>
        <table>
            <tr><th>Nom</th><td><xsl:value-of select="pat:nom"/></td></tr>
            <tr><th>Prenom</th><td><xsl:value-of select="pat:prénom"/></td></tr>
            <tr><th>sexe</th><td><xsl:value-of select="pat:sexe"/></td></tr>
            <tr><th>naissance</th><td><xsl:value-of select="pat:naissance"/></td></tr>
            <tr><th>Numéro de sécurité sociale</th><td><xsl:value-of select="pat:numéroSS"/></td></tr>
            <tr><th>adresse</th><td><xsl:apply-templates select="pat:adresse"/></td></tr>
        </table>
        <h2>Visites</h2>
        <table>
            <tr><th>Date</th><th>Intervenant</th><th>Actes</th></tr>
            <xsl:apply-templates select="pat:visite">
                <xsl:sort select="@date" order="ascending"/>
            </xsl:apply-templates>
        </table>
    </xsl:template>
    
    <!-- Templage visite -->
    <xsl:template match="pat:visite">
        <tr>
            <td><xsl:value-of select="@date"/></td>
            <td>
                <xsl:value-of select="pat:intervenant/pat:nom"/>
                <xsl:text> </xsl:text>
                <xsl:value-of select="pat:intervenant/pat:prénom"/>
            </td>
            <td>
                <xsl:apply-templates select="pat:acte"/>
            </td>
        </tr>
    </xsl:template>
    
    <!-- Template acte -->
    <xsl:template match="pat:acte">
        <li><xsl:value-of select="text()"/></li>
    </xsl:template>
    
    <!-- Template adresse -->
    <xsl:template match="pat:adresse">
        <span>
            <xsl:if test="pat:étage">
                <xsl:value-of select="pat:étage"/><xsl:text>e étage, </xsl:text>
            </xsl:if>
            <xsl:if test="pat:numéro">
                <xsl:value-of select="pat:numéro"/><xsl:text> </xsl:text>
            </xsl:if>
            <xsl:value-of select="pat:rue"/><xsl:text>, </xsl:text>
            <xsl:value-of select="pat:codePostal"/><xsl:text> </xsl:text>
            <xsl:value-of select="pat:ville"/>
        </span>
    </xsl:template>
    
</xsl:stylesheet>