<?php
    
        try {
            $dbh =  new PDO(getenv('APPSETTING_DBServer'), getenv('APPSETTING_DBUser'), getenv('APPSETTING_DBPassword'));
			$dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        } catch(PDOException $e) {
            echo '<h1>An error has ocurred.</h1><pre>', $e->getMessage() ,'</pre>';
        }
 
    $sth = $dbh->prepare('
    SELECT 
    [Id]
	   ,[Name]
      ,[Score]
  FROM [dbo].[Score]
  WHERE Category = :category AND IsMobile = :isMobile
  ORDER BY Score Desc');
	  	  
 		$name=$_GET['name'];
 		$category=$_GET['category'];
 		$isMobile=$_GET['isMobile'];
		
	  $sth->bindParam(':category',$category);
	  $sth->bindParam(':isMobile',$isMobile);
		  
$sth->execute();

		  //$sth->debugDumpParams();
		  
    $sth->setFetchMode(PDO::FETCH_ASSOC);
		
    $result = $sth->fetchAll();

	//echo count($result);

    if(count($result) > 0) {
    	$place = 1;
		$placementId=0;
		$placement='';
        foreach($result as $r) {
        	//echo $place,'.', $r['Name'], "\t", $r['Score'], "\n";
        	if($r['Name']=$name and $r['Id']>$placementId){
				$placementId=$r['Id'];
				$placement= $place.'.'.$r['Name']. "\t". $r['Score'];
			}
			$place+=1;
        }
		echo $placement;
    }
	
	//echo '<h1>FIN</h1>name:',$name,'cat:',$category,'mob:',$isMobile,'count:',count($result);
?>