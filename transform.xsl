<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:output method="xml" indent="yes" encoding="utf-8"/>
  <xsl:strip-space elements="*"/>

  <!-- Ключ для группировки по полному имени (name + surname) -->
  <xsl:key name="employees" match="item" use="concat(@name, '|', @surname)" />

  <xsl:template match="/">
    <Employees>
      <!-- Собираем уникальные сотрудники -->
      <xsl:for-each select="//item[generate-id() = generate-id(key('employees', concat(@name, '|', @surname))[1])]">
        <Employee name="{@name}" surname="{@surname}">
          <!-- Выводим все записи по этому сотруднику -->
          <xsl:for-each select="key('employees', concat(@name, '|', @surname))">
            <salary amount="{@amount}" mount="{@mount}" />
          </xsl:for-each>
        </Employee>
      </xsl:for-each>
    </Employees>
  </xsl:template>

</xsl:stylesheet>