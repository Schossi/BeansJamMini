<?php
    
        try {
            $dbh =  new PDO(getenv('APPSETTING_DBServer'), getenv('APPSETTING_DBUser'), getenv('APPSETTING_DBPassword'));
			$dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        } catch(PDOException $e) {
            echo '<h1>An error has ocurred.</h1><pre>', $e->getMessage() ,'</pre>';
        }
 
    $sth = $dbh->prepare('
    SELECT TOP (10) 
	   [Name]
      ,[Score]
  FROM [dbo].[Score]
  WHERE Category = :category AND IsMobile = :isMobile
  ORDER BY Score Desc');
	  	  
 		$category=$_GET['category'];
 		$isMobile=$_GET['isMobile'];
		
	  $sth->bindParam(':category',$category);
	  $sth->bindParam(':isMobile',$isMobile);
		  
$sth->execute();

		  //$sth->debugDumpParams();
		  
    $sth->setFetchMode(PDO::FETCH_ASSOC);
		
    $result = $sth->fetchAll();

    if(count($result) > 0) {
    	$place = 1;
        foreach($result as $r) {
            echo $place,'.', $r['Name'], "\t", $r['Score'], "\n";
			$place+=1;
        }
    }
	
	//echo '<h1>FIN</h1>cat:',$category,'mob:',$isMobile,'count:',count($result);
?>