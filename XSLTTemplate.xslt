<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    
    <!-- Define the template for the root element (libraryDatabase) -->
    <xsl:template match="/libraryDatabase">
        <html>
            <head>
                <title>Library Database</title>
                <style>
                    table {
                        width: 100%;
                        border-collapse: collapse;
                    }
                    th, td {
                        border: 1px solid #ddd;
                        padding: 8px;
                        text-align: left;
                    }
                    th {
                        background-color: #f2f2f2;
                    }
                </style>
            </head>
            <body>
                <h1>Library Database</h1>
                <table>
                    <tr>
                        <th>Title</th>
                        <th>Author</th>
						<th>Author ID</th>
                        <th>Year</th>
                        <th>Format</th>
                    </tr>

                    <xsl:for-each select="book">
                        <tr>
                            <td><xsl:value-of select="title"/></td>
                            <td>
							    <xsl:value-of select="author"/>
							    <xsl:if test="not(author)">N/A</xsl:if>
							</td>
							<td>
							    <xsl:value-of select="author/@AU_ID"/>
                                <xsl:if test="not(author/@AU_ID)">N/A</xsl:if>
							</td>
                            <td>
                                <xsl:value-of select="year"/>
                                <xsl:if test="not(year)">N/A</xsl:if>
                            </td>
                            <td>
						    <xsl:value-of select="format"/>
							<xsl:if test="not(format)">N/A</xsl:if>
						</td>
                        </tr>
                    </xsl:for-each>
                </table>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>
