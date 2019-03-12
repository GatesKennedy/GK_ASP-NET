//==================================================
//=============  EXTRACT URLS  =====================
//==================================================
function extractLinkUrls() {
  var body = DocumentApp.getActiveDocument().getBody();
  var numChildren = body.getNumChildren();
  var output = [];
  Logger.log("numChil: " +numChildren);
  // Walk through all the child elements of the body
  for (var li=0; li<numChildren; li++) {
    Logger.log("=====LINE LOOP: " + li);
    var line = body.getChild(li).asText();
    if (line.getType() == DocumentApp.ElementType.PARAGRAPH) {
      var textObj = line.editAsText();
      var text = line.getText();
      var inUrl = false;

      for (var ch=0; ch < text.length; ch++) {
        //Logger.log("==Char Loop: " + ch);
        //==========
        var url = textObj.getLinkUrl(ch);
        //==========
        if (url != null) {
          Logger.log("Cond: URL = NotNull");
          if (!inUrl) {
            Logger.log("vv  inUrl == FALSE");
            // We are now!
            var curUrl = {};
            inUrl = true;
            output.push(url);
            var beenThere = line.editAsText().findText('!!!')  ;
            if (beenThere == null){
              line.setText("VIDEO~~" + text + "~~" + url + "\r");
            }
          }
          else {
            Logger.log("Cond: InURL = FALSE");
            curUrl.endOffsetInclusive = ch;
          }          
        }
        else {
          Logger.log("Cond: URL == NULL");
          if (inUrl) {
            Logger.log("    InURL = TRUE")
            // Not any more, we're not.
            inUrl = false;
            curUrl = {};
          }
        }
      }
    }
  }
  var html = output.join('\r');
  Logger.log(html);
  //emailHtml(html, images);
  //createDocumentForHtml(html, images);
}             

//===============================================================
//=============  MAIN METHOD  ===================================
//===============================================================
function EmailAsParseForm() {
    var body = DocumentApp.getActiveDocument().getBody();
    var numChildren = body.getNumChildren();
    var output = [];
    var images = [];
    var listCounters = {};
    //  PageBreak Var
    var pageBrkID = 0;
    var snagged = false;
  
    // Walk through lines...Add to 'output[]'.
    for (var i = 0; i < numChildren; i++) {
        //  Logger.log("snagged = " + snagged + "\r PageBrkID = "+ pageBrkID);
        var itemCurr = body.getChild(i);
        var itemPrev = "";
        //  GET MAIN ListID
        if(!snagged){
            Logger.log("S N A G G E D");
            Logger.log("List ID: " + pageBrkID)
            snagged = true;
            pageBrkID = itemCurr.getListId();
        }        
        output.push(processItem(itemCurr, itemPrev, listCounters, images, output, pageBrkID, snagged));
    }

    // Compile 'output[]' Array
    var html = output.join('\n');
    emailHtml(html, images);
    createDocumentForHtml(html, images);
  }
//===============================================================
//  EMAIL HTML
  function emailHtml(html, images) {

    //  Images Attachments 
    var attachments = [];
    for (var j=0; j<images.length; j++) {
      attachments.push( {
        "fileName": images[j].name,
        "mimeType": images[j].type,
        "content": images[j].blob.getBytes() } );
    }
    //  Images Inline
    var inlineImages = {};
    for (var j=0; j<images.length; j++) {
      inlineImages[ [images[j].name] ] = images[j].blob;
    }
    //  Compile Email 
    var name = DocumentApp.getActiveDocument().getName()+".txt";
    attachments.push({"fileName":name, "mimeType": "text/html", "content": html});
    //  Send Email
    MailApp.sendEmail({
       to: Session.getActiveUser().getEmail(),
       subject: name,
       htmlBody: html,
       inlineImages: inlineImages,
       attachments: attachments
     });
  }
//===============================================================
//  Create Document
  function createDocumentForHtml(html, images) {
    var name = DocumentApp.getActiveDocument().getName()+".txt";
    var newDoc = DocumentApp.create(name);
    //  Insert Text Content
    newDoc.getBody().setText(html);
    //  Append Images to Doc
    for(var j=0; j < images.length; j++)
      newDoc.getBody().appendImage(images[j].blob);
    newDoc.saveAndClose();
  }
//===============================================================
//  DUMP Attributes?
/*
  function dumpAttributes(atts) {
    // Log the paragraph attributes.
    for (var att in atts) {
      Logger.log(att + ":" + atts[att]);
    }
  }
  */
  
//============================================================
//          PROCESS ITEM
//============================================================
function processItem(itemCurr, itemPrev, listCounters, images, output, pageBrkID, snagged) {
    var eleOutput = [];
    var prefix = ""; 
    var suffix = "";
    var textAtts = itemCurr.getAttributes();

    eleOutput.push('');
    //_____________
    // [1]MODULE
    if (itemCurr.getType() == DocumentApp.ElementType.PARAGRAPH 
    && itemCurr.getNumChildren() != 0 
    && itemCurr.getText().trim() != ""
    && itemCurr.getText().trim() != "QUIZ:") {
        prefix = "", suffix = "";
    if (itemCurr.getNumChildren() == 0)
        return "";
    }
    //_________________
    // [1]INLINE_IMAGE
    else if (itemCurr.getType() == DocumentApp.ElementType.INLINE_IMAGE) {
        Logger.log("IS Image!");
        processImage(itemCurr, images, eleOutput);
    }
    //_______________
    // [1]LIST_ITEMS
    // *PAGE BREAKS*
    else if (itemCurr.getType()===DocumentApp.ElementType.LIST_ITEM) { 
        var listItem = itemCurr;
        var itemText = itemCurr.getText().trim();
        var gt = listItem.getGlyphType();
        var key = listItem.getListId() + '.' + listItem.getNestingLevel();
        var counter = listCounters[key] || 0;
        var itemIdCurr = itemCurr.getListId();
        var pageFlag = "$$$";

        // LOG GYPH TYPE
        //Logger.log("GLYPH: " + gt)
        
        // FIND PAGE BREAKS
        if (itemIdCurr === pageBrkID) {
            return pageFlag + itemText;
        }
        // [1.1] FIRST.FIRST ITEM
        //if ( counter == 0 ) {

            // Bullet => '~~'
            if (gt === DocumentApp.GlyphType.BULLET
                || gt === DocumentApp.GlyphType.HOLLOW_BULLET
                || gt === DocumentApp.GlyphType.SQUARE_BULLET) {
                // Learning Objectives
                if (textAtts.FOREGROUND_COLOR) prefix = '%%';
                else prefix = '~~';
            }
            // Numbered => '^^'
            else if (gt === DocumentApp.GlyphType.NUMBER) prefix = "^^";
            // Alpha'd => '##'
            else if (gt === DocumentApp.GlyphType.LATIN_LOWER) prefix = "##";
            else prefix = "~~";
        //}
        //[1.2] MIDDLE.MIDDLE ITEM
        /*
        else {
            // Bullet => '~~'
            if (gt === DocumentApp.GlyphType.BULLET
                || gt === DocumentApp.GlyphType.HOLLOW_BULLET
                || gt === DocumentApp.GlyphType.SQUARE_BULLET) {
                // Learning Objectives
                if (textAtts.FOREGROUND_COLOR) prefix = '%%';
                else  prefix = "";
            }
            // Numbered => '^^'
            else if (gt === DocumentApp.GlyphType.NUMBER) prefix = "^^";
            // Alpha'd => '##'
            else if (gt === DocumentApp.GlyphType.LATIN_LOWER) prefix = "##"
            else prefix = "~~";
        }
        // [1.3] LAST.LAST ITEM
        if (itemCurr.isAtDocumentEnd() || (itemCurr.getNextSibling() && (itemCurr.getNextSibling().getType() != DocumentApp.ElementType.LIST_ITEM))) {
        // Bullet => '~~'
            if (gt === DocumentApp.GlyphType.BULLET
                || gt === DocumentApp.GlyphType.HOLLOW_BULLET
                || gt === DocumentApp.GlyphType.SQUARE_BULLET) {
                // Learning Objectives
                if (textAtts.FOREGROUND_COLOR) prefix = '%%';
                else prefix = '~~';
            }
            // Numbered => '^^'
            else if (gt === DocumentApp.GlyphType.NUMBER) prefix = "^^";
            // Alpha'd => '##'
            else if (gt === DocumentApp.GlyphType.LATIN_LOWER) prefix = "##"
            else prefix = "~~";
        }
        */
        counter++;
        listCounters[key] = counter;
    }
    //_______________
    //***** PUSH PREFIX
    if(prefix != "") eleOutput.push(prefix);
        
    //______________
    // [2] IS TEXT
    if (itemCurr.getType() == DocumentApp.ElementType.TEXT) {
    processText(itemCurr, eleOutput);
    }
    //_____________
    // [2] NOT TEXT
    else {
        if (itemCurr.getNumChildren) {
            var numChildren = itemCurr.getNumChildren();

            // Loop through all the child elements of Current Item
            for (var i = 0; i < numChildren; i++) {
            var child = itemCurr.getChild(i);
            //**** PUSH Current Item Child
            eleOutput.push(processItem(child, listCounters, images));
            }
        }
    }

    //***** PUSH SUFFIX
    eleOutput.push(suffix);
    return eleOutput.join('');
}
  
  //============================================================
  //          PROCESS TEXT
  //============================================================
  function processText(item, eleOutput) {
    var text = item.getText();
    var indices = item.getTextAttributeIndices();
    var textAtts = item.getAttributes();
    var color = textAtts.FOREGROUND_COLOR;
    var TEXT_COLOR = "#38761d";

    if (textAtts.FOREGROUND_COLOR == TEXT_COLOR) {
      Logger.log("TEXT COLOR MMMAAATTTCCCHHH: " + textAtts.FOREGROUND_COLOR);
      eleOutput.push("^G^ ");
    }
    if (indices.length <= 1) {
      /*
      // Assuming that a whole para fully italic is a quote
      if(item.isBold()) {
        eleOutput.push('<strong>' + text + '</strong>');
      }
      else if(item.isItalic()) {
        eleOutput.push('<blockquote>' + text + '</blockquote>');
      }
      */
      if (text.trim().indexOf('http://') == 0) {
        eleOutput.push('<a href="' + text + '" rel="nofollow">' + text + '</a>');
      }
      else {
        eleOutput.push(text);
      }
    }
    else {
      
      for (var i=0; i < indices.length; i ++) {
        var partAtts = item.getAttributes(indices[i]);
        var startPos = indices[i];
        var endPos = i+1 < indices.length ? indices[i+1]: text.length;
        var partText = text.substring(startPos, endPos);

  /*
        if (partAtts.ITALIC) {
          eleOutput.push('<i>');
        }
        if (partAtts.BOLD) {
          eleOutput.push('<strong>');
        }
        if (partAtts.UNDERLINE) {
          eleOutput.push('<u>');
        }
  */
        // If someone has written [xxx] and made this whole text some special font, like superscript
        // then treat it as a reference and make it superscript.
        // Unfortunately in Google Docs, there's no way to detect superscript
        if (partText.indexOf('[')==0 && partText[partText.length-1] == ']') {
          eleOutput.push('<sup>' + partText + '</sup>');
        }
        else if (partText.trim().indexOf('http://') == 0) {
          eleOutput.push('<a href="' + partText + '" rel="nofollow">' + partText + '</a>');
        }
        else {
          eleOutput.push(partText);
        }
  /*
        if (partAtts.ITALIC) {
          eleOutput.push('</i>');
        }
        if (partAtts.BOLD) {
          eleOutput.push('</strong>');
        }
        if (partAtts.UNDERLINE) {
          eleOutput.push('</u>');
        }
  */
      }
    }
  }
  
  //============================================================
  //          PROCESS IMAGE
  //============================================================
  function processImage(item, images, eleOutput)
  {
    Logger.log("ENTER FXN: processImage()");
    images = images || [];
    var blob = item.getBlob();
    var contentType = blob.getContentType();
    var extension = "";
    if (/\/png$/.test(contentType)) {
      extension = ".png";
    } else if (/\/gif$/.test(contentType)) {
      extension = ".gif";
    } else if (/\/jpe?g$/.test(contentType)) {
      extension = ".jpg";
    } else {
      throw "Unsupported image type: "+contentType;
    }
    var imagePrefix = "IMAGE_";
    var imageCounter = images.length;
    var name = imagePrefix + imageCounter + extension;
    imageCounter++;
    eleOutput.push('<img src="cid:'+name+'" />');
    images.push( {
      "blob": blob,
      "type": contentType,
      "name": name});
      Logger.log("ImageName = " + name);
  }
  