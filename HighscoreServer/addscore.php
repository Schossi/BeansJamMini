<?php 
        $secretKey = getenv('APPSETTING_SecretKey'); // Change this value to match the value stored in the client javascript below 
 
        try {
            $dbh =  new PDO(getenv('APPSETTING_DBServer'), getenv('APPSETTING_DBUser'), getenv('APPSETTING_DBPassword'));
			$dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        } catch(PDOException $e) {
            echo '<h1>An error has ocurred.</h1><pre>', $e->getMessage() ,'</pre>';
        }
 
 		$name=$_GET['name'];
 		$score=$_GET['score'];
 		$category=$_GET['category'];
 		$isMobile=$_GET['isMobile'];
 		$hash=$_GET['hash'];
 
        $realHash = md5($name . $score . $secretKey); 
        if($realHash == $hash) { 
            $sth = $dbh->prepare('
     INSERT INTO [dbo].[Score]
           ([Name]
           ,[Score]
           ,[Category]
           ,[IsMobile])
     VALUES
           (:name
           ,:score
           ,:category
           ,:isMobile)');
		   
		   $sth->bindParam(':name',$name);
		   $sth->bindParam(':score',$score);
		   $sth->bindParam(':category',$category);
		   $sth->bindParam(':isMobile',$isMobile);
		   
		   //$params=array(':name',$name,'score',$score,'category',$category,'isMobile',$isMobile);
		   
		   if (!$sth) {
			    echo "\nPDO::errorInfo():\n";
			    print_r($dbh->errorInfo());
			}
		   
            try {
                $sth->execute();
				//$recId->lastInsertId();
				
							
			/*	
    $sth = $dbh->prepare('
    SELECT  
	   [Name]
      ,[Score]
  FROM [dbo].[Score]
  WHERE Category = :category AND IsMobile = :isMobile AND Id=:id
  ORDER BY Score Desc');
	  	
	  $sth->bindParam(':category',$category);
	  $sth->bindParam(':isMobile',$isMobile);
	  $sth->bindParam(':id',$recId);
		  
$sth->execute();
				
    $sth->setFetchMode(PDO::FETCH_ASSOC);
		
    $result = $sth->fetchAll();
	
	
    if(count($result) > 0) {
    	$place = 1;
        foreach($result as $r) {
            if($r['Id']=$recId){
            	echo $place;
				return;
            }
			$place+=1;
        }
    }

echo '-1';
				
				*/
				
            } catch(Exception $e) {
                echo '<h1>An error has ocurred.</h1><pre>', $e->getMessage() ,'</pre>';
            }   
        } 
		else {						
                echo '<h1>WRONG MD5</h1><pre>real:',$realHash,'hash:',$hash,'</pre>';
		}
?>