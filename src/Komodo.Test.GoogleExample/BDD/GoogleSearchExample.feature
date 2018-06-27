Feature: Google Search Example
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@web
Scenario: Google search example 
	Given user navigates to url 'http://www.google.com'
	Then pause 3000
	
@web
Scenario Outline: Sainsburys search example 
	Given user navigates to url 'https://www.sainsburys.co.uk/webapp/wcs/stores/servlet/SearchDisplayView?catalogId=10123&langId=44&storeId=10151&krypto=o3Ivgm1sN33M1OwzXbuRBdcKTpcK2P%2FU3ErDKwEaqjN%2BJWdQTMTl44n6GMMEgzC5bcapxMgly0Wx1U3msKRoKpyQyZPDcU3D8LYSFBit5O%2F6Ovo0m6NKly59tO7ueCGSnDPQgcikaGE5RyN86uHprlWeydzwTs0sU0FkaYrorq%2B%2B9b%2FuZzcTbEjwYi0yV4iP#langId=44&storeId=10151&catalogId=10123&categoryId=&parent_category_rn=&top_category=&pageSize=36&orderBy=RELEVANCE&searchTerm=<searchTerm>&beginIndex=0&categoryFacetId1='
	Then pause 30000

	Examples: 
		| searchTerm |
		| pop        |
		| rice       |