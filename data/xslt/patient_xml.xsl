<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:cab="http://www.univ-grenoble-alpes.fr/l3miage/medical"
                xmlns:act="http://www.univ-grenoble-alpes.fr/l3miage/actes"
                xmlns="http://www.univ-grenoble-alpes.fr/l3miage/patient"
                exclude-result-prefixes="cab act"
>
    <!-- Remarque: Le namespace par défaut va être rajouter au xml résultat comme un default namespace -->
    
    <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
    
    <xsl:param name="destinedName">Omar</xsl:param>
    <xsl:variable name="actes" select="document('../xml/actes.xml', /)/act:ngap"/>
    
    <xsl:template match="/">
        <xsl:variable name="lePatient" select="//cab:patient[cab:prénom = $destinedName]"/>
        
        <xsl:text>&#10;</xsl:text> <!-- insérer un retour à la ligne -->
        <patient xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                 xsi:schemaLocation="http://www.univ-grenoble-alpes.fr/l3miage/patient ../xsd/patient.xsd"
        >
            <nom><xsl:value-of select="$lePatient/cab:nom"/></nom>
            <prénom><xsl:value-of select="$destinedName"/></prénom>
            <sexe><xsl:value-of select="$lePatient/cab:sexe"/></sexe>
            <naissance><xsl:value-of select="$lePatient/cab:naissance"/></naissance>
            <numéroSS><xsl:value-of select="$lePatient/cab:numéro"/></numéroSS>
            <adresse>
                <xsl:if test="$lePatient/cab:adresse/cab:étage">
                    <étage><xsl:value-of select="$lePatient/cab:adresse/cab:étage"/></étage>
                </xsl:if>
                <xsl:if test="$lePatient/cab:adresse/cab:numéro">
                    <numéro><xsl:value-of select="$lePatient/cab:adresse/cab:numéro"/></numéro>
                </xsl:if>
                <rue><xsl:value-of select="$lePatient/cab:adresse/cab:rue"/></rue>
                <codePostal><xsl:value-of select="$lePatient/cab:adresse/cab:codePostal"/></codePostal>
                <ville><xsl:value-of select="$lePatient/cab:adresse/cab:ville"/></ville>
            </adresse>
            <xsl:apply-templates select="$lePatient/cab:visite">
                <xsl:sort select="@date" order="ascending"/>
            </xsl:apply-templates>
        </patient>
    </xsl:template>
    
    <!-- Template visite -->
    <xsl:template match="cab:visite">
        <xsl:variable name="idIntervenant" select="@intervenant"/>
        <xsl:variable name="infoIntervenant" select="//cab:infirmier[@id = $idIntervenant]"/>
        <visite>
            <xsl:attribute name="date">
                <xsl:value-of select="@date"/>
            </xsl:attribute>
            <intervenant>
                <nom><xsl:value-of select="$infoIntervenant/cab:nom"/></nom>
                <prénom><xsl:value-of select="$infoIntervenant/cab:prénom"/></prénom>
            </intervenant>
            <xsl:apply-templates select="cab:acte"/>
        </visite>
    </xsl:template>

    <!-- Template acte -->
    <xsl:template match="cab:acte">
        <xsl:variable name="idActe" select="@id"/>
        <acte><xsl:value-of select="$actes/*/act:acte[@id=$idActe]/text()"/></acte>
    </xsl:template>
    
</xsl:stylesheet>